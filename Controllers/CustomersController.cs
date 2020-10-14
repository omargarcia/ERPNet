﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ERPNet.Data;
using ERPNet.Models;
using Microsoft.AspNetCore.Cors;

namespace ERPNet.Controllers
{
    [EnableCors ( "AllowSpecificOrigin" )]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ERPNetContext _context;

        public CustomersController(ERPNetContext context)
        {
            _context = context;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomer()
        {
            //return await _context.Customer.ToListAsync();

            return await _context.Customer
               .Include ( p => p.Person )
               .Include( p => p.Orders)
               .ToListAsync ();
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            //var customer = await _context.Customer.FindAsync(id);

            var customer = await _context.Customer
               .Include ( c => c.Person )
               .SingleOrDefaultAsync ( c => c.PersonId == id );

            if(customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {
            if (id != customer.Id)
            {
                return BadRequest();
            }

            var editCustomer = await _context.Customer.FindAsync ( id );

            if(editCustomer == null)
            {
                return NotFound ();
            }

            var person = await _context.Person.FindAsync ( editCustomer.PersonId );

            if(person == null)
            {
                return NotFound ();
            }

            person.Name = customer.Person.Name;
            person.LastName = customer.Person.LastName;

            _context.Entry( person ).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Customers
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            //_context.Customer.Add(customer);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetCustomer", new { id = customer.CustomerId }, customer);

            var person = new Person
            {
                Name = customer.Person.Name,
                LastName = customer.Person.LastName
            };

            _context.Person.Add ( person );
            await _context.SaveChangesAsync ();

            var newCustomer = new Customer
            {
                PersonId = customer.PersonId
            };

            _context.Customer.Add ( newCustomer );
            await _context.SaveChangesAsync ();
            return CreatedAtAction ( "GetCustomer", new { id = customer.Id }, newCustomer );

        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Customer>> DeleteCustomer(int id)
        {
            var customer = await _context.Customer.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customer.Remove(customer);
            await _context.SaveChangesAsync();

            return customer;
        }

        private bool CustomerExists(int id)
        {
            return _context.Customer.Any(e => e.Id == id);
        }
    }
}
