using PedidosYa.BusinessLogic;
using PedidosYa.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace PedidosYa.WebApi.Controllers
{
    public class CategoryController : ApiController
    {
        CategoryLogic categoryLogic = new CategoryLogic();
        // GET: api/Category
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Category/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Category
        [HttpPost]
        public IHttpActionResult Post([FromBody]Category category)
        {
            try
            {
                categoryLogic.CreateCategory(category);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
