using GeneralStoreAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace GeneralStoreAPI.Controllers {
    public class CustomerController : ApiController {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();
        //private readonly IdentityModels _context1 = new IdentityModels();

        //Post
        [HttpPost]
        public async Task<IHttpActionResult> CreateCustomer([FromBody] Customer model) {
            if (model is null)
                return BadRequest("Your request body was empty.  Please try again.");
            if (ModelState.IsValid) {
                _context.Customers.Add(model);
                await _context.SaveChangesAsync();

                return Ok("Successfully added customer to database.");
            }
            return BadRequest(ModelState);
        }

        //Get ALL
        [HttpGet]
        public async Task<IHttpActionResult> GetAllCustomers() {
            List<Customer> customers = await _context.Customers.ToListAsync();
            return Ok(customers);
        }

        //Get
        [HttpGet]
        public async Task<IHttpActionResult> GetCustomerById ([FromUri] int id) {
            Customer customer = await _context.Customers.FindAsync(id);
            if(customer != null) {
                return Ok(customer);
            }
            return NotFound();
        }
        //Put
        [HttpPut]
        public async Task<IHttpActionResult> UpdateCustomerById([FromUri] int id, [FromBody] Customer updatedInfo) {
        if (id != updatedInfo?.CustomerId) {
                return BadRequest("Ids do not match");
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Customer customer = await _context.Customers.FindAsync(id);

            if (customer is null)
                return NotFound();

            customer.FirstName = updatedInfo.FirstName;
            customer.LastName = updatedInfo.LastName;

            await _context.SaveChangesAsync();
            return Ok("Update Successful");
        }

        //Delete
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteCustomer ([FromUri] int id) {
            Customer customer = await _context.Customers.FindAsync(id);

            if (customer is null)
                return NotFound();
            _context.Customers.Remove(customer);

            if (await _context.SaveChangesAsync() != 0)
                return Ok("Delete Successful");

            return InternalServerError();
        }
    }
}
