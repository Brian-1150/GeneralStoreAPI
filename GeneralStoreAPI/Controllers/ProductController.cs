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
        [Route("api/Product/{sku}")] //Defined Route so I did not get the sku=?{sku}
        [HttpGet]
        public async Task<IHttpActionResult> GetProductBySku(string sku) {
            Product product = await _context.Products.FindAsync(sku);
            if (product is null)
                return NotFound();
            return Ok(product);

        }

        //Put
        [Route("api/Product/{sku}")] //Defined Route so I did not get the sku=?{sku}
        [HttpPut]
        public async Task<IHttpActionResult> EditProduct([FromUri] string sku, [FromBody] Product UpdatedProduct) {
            if (sku != UpdatedProduct?.SKU)
                return BadRequest("SKUs do not match");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Product product = await _context.Products.FindAsync(sku);

            if (product is null)
                return NotFound();

            product.Name = UpdatedProduct.Name;
            product.Cost = UpdatedProduct.Cost;
            product.NumberInInventory = UpdatedProduct.NumberInInventory;

            await _context.SaveChangesAsync();
            return Ok("Update was successful");
        }

        //Delete
        [Route("api/Product/{sku}")] //Defined Route so I did not get the sku=?{sku}
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteProduct([FromUri] string sku) {
            Product product = await _context.Products.FindAsync(sku);
            if(product != null)
            _context.Products.Remove(product);

            if (await _context.SaveChangesAsync() == 1)
                return  Ok("Delete Successful");

            return InternalServerError();

        }
    }
}
