﻿using System;
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
//using System.Web.OData.Routing;

using HLF.ERP.Service.Database;

namespace HLF.ERP.Service.Controllers
{
    
    public class CategoriesController : ODataController
    {
        private Northwind db = new Northwind();

        // GET: odata/Categories
        [EnableQuery]
        [Authorize]
        public IQueryable<Category> GetCategories()
        {
            return db.Categories;
        }

        // GET: odata/Categories(5)
        [EnableQuery]
        [Authorize]
        public SingleResult<Category> GetCategory([FromODataUri] int key)
        {
            return SingleResult.Create(db.Categories.Where(category => category.CategoryID == key));
        }

        // PUT: odata/Categories(5)
        [Authorize]
        public IHttpActionResult Put([FromODataUri] int key, Delta<Category> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Category category = db.Categories.Find(key);
            if (category == null)
            {
                return NotFound();
            }

            patch.Put(category);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(category);
        }

        // POST: odata/Categories
        [Authorize]
        public IHttpActionResult Post(Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Categories.Add(category);
            db.SaveChanges();

            return Created(category);
        }

        // PATCH: odata/Categories(5)
        [AcceptVerbs("PATCH", "MERGE")]
        [Authorize]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<Category> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Category category = db.Categories.Find(key);
            if (category == null)
            {
                return NotFound();
            }

            patch.Patch(category);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(category);
        }

        // DELETE: odata/Categories(5)
        [Authorize]
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            Category category = db.Categories.Find(key);
            if (category == null)
            {
                return NotFound();
            }

            db.Categories.Remove(category);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Categories(5)/Products
        [EnableQuery]
        [Authorize]
        public IQueryable<Product> GetProducts([FromODataUri] int key)
        {
            return db.Categories.Where(m => m.CategoryID == key).SelectMany(m => m.Products);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CategoryExists(int key)
        {
            return db.Categories.Count(e => e.CategoryID == key) > 0;
        }
    }
}
