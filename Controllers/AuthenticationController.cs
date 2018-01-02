using Microsoft.AspNetCore.Mvc;
using haibara.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Collections.Generic;
using System.Linq;

namespace haibara.Controllers
{
    public class AuthenticationController
    {
        private readonly HaibaraContext _context;

        public AuthenticationController(HaibaraContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [Route("api/[controller]/RequestToken")]
        [HttpPost]
        public IActionResult RequestToken([FromBody] TokenRequest request)
        {
            if (request.Username == "konata" && request.Password == "qwert123")
            {
                var claims = new Claim[]
                {
                    new Claim(ClaimTypes.Name, request.Username)
                };
                Dictionary<string, string> configurations = Configurations.ReturnConfigurations();
                string securityKey = configurations["SecurityKey"];
                SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
                SigningCredentials cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                JwtSecurityToken token = new JwtSecurityToken(
                    issuer: "localhost:5000",
                    audience: "localhost:5000",
                    claims: claims,
                    expires: DateTime.Now.AddHours(12),
                    signingCredentials: cred
                );

                return new OkObjectResult(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }
            return new BadRequestObjectResult("Could not Determine Username/Password Combination");
        }

        //[Authorize(AuthenticationSchemes = "Bearer")]
        [Route("api/[controller]/AddUser")]
        [HttpPost]
        public IActionResult AddUser([FromBody] UserCredentials userCredentials)
        {
            List<UserInformation> userInformations = _context.UserInformations.ToList();
            bool userNameExists = userInformations.Any(ui => ui.Username == userCredentials.Username);
            if (userNameExists)
            {
                return new BadRequestObjectResult("Username Already Exists");
            }
            else
            {
                string hashedPassword = BCrypt.BCryptHelper.HashPassword(userCredentials.Password, BCrypt.SaltRevision.Revision2A.ToString());
                UserInformation userInformation = new UserInformation()
                {
                    Username = userCredentials.Username,
                    HashedPassword = hashedPassword
                };
                _context.UserInformations.Add(userInformation);
                _context.SaveChanges();
                return new CreatedAtRouteResult("AuthenticationController", userInformation);
            }
        }

    }
}