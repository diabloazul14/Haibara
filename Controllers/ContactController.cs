using Microsoft.AspNetCore.Mvc;
using haibara.Models;
using System.Linq;
using System.Collections.Generic;

namespace haibara.Controllers
{
    [Route("api/[controller]")]
    public class ContactController
    {
        private readonly HaibaraContext _context;

        public ContactController(HaibaraContext context)
        {
            _context = context;

            if (_context.Contacts.Count() == 0)
            {
                _context.Contacts.Add(new Contact
                {
                    Name = "Matthew",
                    Address = "237 Backs Ln. Apt. D",
                    Email = "diabloazul14@gmail.com",
                    Phone = "7142225074",
                    Other = "Is a Software Engineer"
                });
                _context.SaveChanges();
            }
        }

        //Returns All Contacts
        [HttpGet]
        public IEnumerable<Contact> GetAll()
        {
            return _context.Contacts.ToList();
        }

        //Returns Contact By Id //Used in Create method
        [HttpGet("{id}", Name = "GetContact")]
        public IActionResult GetById(long id)
        {
            Contact contact = _context.Contacts.FirstOrDefault(c => c.Id == id);

            if (contact == null)
                return new NotFoundResult();

            return new ObjectResult(contact);
        }

        //Adds Contacts
        [HttpPost]
        public IActionResult Create([FromBody] Contact contact)
        {
            if (contact == null)
                return new BadRequestResult();

            _context.Contacts.Add(contact);
            _context.SaveChanges();

            return new CreatedAtRouteResult("GetContact", new { id = contact.Id }, contact);
        }


        //Updates Contact
        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] Contact contact)
        {
            if (contact == null)
                return new BadRequestResult();

            Contact dbContact = _context.Contacts.FirstOrDefault(c => c.Id == id);

            if (dbContact == null)
                return new NotFoundResult();

            dbContact.Name = contact.Name;
            dbContact.Address = contact.Address;
            dbContact.Email = contact.Email;
            dbContact.Phone = contact.Phone;
            dbContact.Other = contact.Other;

            _context.Contacts.Update(dbContact);
            _context.SaveChanges();

            return new NoContentResult();            
        }

        //Deletes Contact
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            Contact dbContact = _context.Contacts.FirstOrDefault(c => c.Id == id);

            if (dbContact == null)
                return new NotFoundResult();

            _context.Contacts.Remove(dbContact);
            _context.SaveChanges();

            return new NoContentResult();            
        }

    }
}