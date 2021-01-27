using Extensions;
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
    public class ProductController : ApiController {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        //Post
        [HttpPost]
        public async Task<IHttpActionResult> CreateProduct([FromBody] Product model) {
            if (model is null)
                return BadRequest("Request body empty");

            if (model.SKU.Length != 8 || model.SKU.Contains(" ") || !model.SKU.IsAlphaNumeric())  //used extentions NuGet Package)
                return BadRequest("SKU must be a unique sequence of 8 alpha-numeric digits with no spaces. ");
            if (ModelState.IsValid) {
                model.SKU.ToUpper();
                _context.Products.Add(model);
                await _context.SaveChangesAsync();

                return Ok("Success");
            }
            return BadRequest("is this the problem?");
        }

        //Get ALL
        [HttpGet]
        public async Task<IHttpActionResult> GetAllProducts() {
            List<Product> products = await _context.Products.ToListAsync();
            return Ok(products);
        }

        //Get
        [HttpGet]
        public async Task<IHttpActionResult> GetProductBySku(string sku) {
            Product product = await _context.Products.FindAsync(sku);
            if (product is null)
                return NotFound();
            return Ok(product);

        }

        //Put

        //Delete
    }
}
