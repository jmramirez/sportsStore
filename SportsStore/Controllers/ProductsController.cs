using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace SportsStore.Controllers
{
    public class ProductsController : ApiController
    {
        public ProductsController()
        {
            Repository = (IRepository)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IRepository));
        }

        public IEnumerable<Product> GetProducts()
        {
            return Repository.Products;
        }

        public Product GetProduct(int id)
        {
            Product result = Repository.Products.Where(p => p.Id == id).FirstOrDefault();
            if(result == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            else
            {
                return result;
            }
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IHttpActionResult> PostProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                await Repository.SaveProductAsync(product);
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [Authorize(Roles = "Administrators")]
        public async Task DeletePRoduct(int id)
        {
            await Repository.DeleteProductAsync(id);
        }

        private IRepository Repository { get; set; }
    }
}
