using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;

//using System.Web.OData;
//using System.Web.OData.Extensions;
//using System.Web.OData.Routing;


using HLF.ERP.Service.Database;
using HLF.ERP.Services.Models;
using HLF.ERP.Services.Infrastructure;

namespace HLF.ERP.Service.Controllers
{
   
    public class ProductsController : ODataController
    {
        private Northwind db = new Northwind();


        private readonly ILogger _logger;

        public ProductsController(ILogger logger)
        {
            this._logger = logger;
        }



        [HttpGet]
        public IHttpActionResult MostExpensive()
        {


            var productDetail = db.Database.SqlQuery<ProductDetail>("EXEC [dbo].[Ten Most Expensive Products]");
            return Ok(productDetail);
        }

        [HttpGet]
        //[ODataRoute("GetSalesTaxRate(PostalCode={postalCode})")]
        public IHttpActionResult GetSalesTaxRate([FromODataUri] int postalCode)
        {
            double rate = 5.6;  // Use a fake number for the sample.
            return Ok(rate);
        }

        // GET: odata/Products
        [EnableQuery]
        [Authorize]
        public IQueryable<Product> GetProducts()
        {
            _logger.Write("Inside Get Products");
            return db.Products;
        }

        // GET: odata/Products(5)
        [EnableQuery]
        [Authorize]
        public SingleResult<Product> GetProduct([FromODataUri] int key)
        {
            return SingleResult.Create(db.Products.Where(product => product.ProductID == key));
        }


        
        // PUT: odata/Products(5)
        [Authorize]
        public IHttpActionResult Put([FromODataUri] int key, Delta<Product> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Product product = db.Products.Find(key);
            if (product == null)
            {
                return NotFound();
            }

            patch.Put(product);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(product);
        }


        // POST: odata/Products
        [Authorize]
        public IHttpActionResult Post(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Products.Add(product);
            db.SaveChanges();

            return Created(product);
        }



        // PATCH: odata/Products(5)
        [Authorize]
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<Product> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Product product = db.Products.Find(key);
            if (product == null)
            {
                return NotFound();
            }

            patch.Patch(product);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(product);
        }

        // DELETE: odata/Products(5)
        [Authorize]
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            Product product = db.Products.Find(key);
            if (product == null)
            {
                return NotFound();
            }

            db.Products.Remove(product);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Products(5)/Category
        [EnableQuery]
        [Authorize]
        public SingleResult<Category> GetCategory([FromODataUri] int key)
        {
            return SingleResult.Create(db.Products.Where(m => m.ProductID == key).Select(m => m.Category));
        }

        // GET: odata/Products(5)/Order_Details
        [EnableQuery]
        [Authorize]
        public IQueryable<Order_Detail> GetOrder_Details([FromODataUri] int key)
        {
            return db.Products.Where(m => m.ProductID == key).SelectMany(m => m.Order_Details);
        }

        // GET: odata/Products(5)/Supplier
        [EnableQuery]
        [Authorize]
        public SingleResult<Supplier> GetSupplier([FromODataUri] int key)
        {
            return SingleResult.Create(db.Products.Where(m => m.ProductID == key).Select(m => m.Supplier));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int key)
        {
            return db.Products.Count(e => e.ProductID == key) > 0;
        }
    }
}
