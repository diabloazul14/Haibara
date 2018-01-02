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
    [Route("api/[controller]")]
    public class AuthenticationController
    {
        private readonly HaibaraContext _context;

        public AuthenticationController(HaibaraContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
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

        [HttpPost]
        {
            IEnumerable<UserInformation> userInformations =  _context.UserInformations.AsEnumerable();
        public IActionResult AddUser([FromBody] UserCredentials userCredentials)
            bool userNameExists = userInformations.Any(ui => ui.Username == userCredentials.Username);
            if (userNameExists)
            {
                return new BadRequestObjectResult("Username Already Exists");
            }
            else 
                //Add the User straight                
            {
                //Encrpyt the Password
                _context.SaveChanges();
                return new CreatedAtRouteResult("AuthenticationController", new { id = contact.Id }, contact);
            }
        }

    }
}