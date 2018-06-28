using PedidosYa.BusinessLogic;
using PedidosYa.DTO;
using PedidosYa.Exceptions;
using PedidosYa.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PedidosYa.WebApi.Controllers
{
  
    public class RestaurantController : ApiController
    {
        IRestaurantLogic restaurantLogic = new RestaurantLogic();
        // GET: api/Restaurant
        public IHttpActionResult Get()
        {
            try
            {
                IEnumerable<Restaurant> restaurants = restaurantLogic.GetAllRestaurants();
                if (restaurants == null)
                {
                    return NotFound();
                }
                return Ok(restaurants);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // GET: api/Restaurant/5
        [HttpGet]
        public IHttpActionResult GetById(int idRestaurant)
        {
            try
            {
                Restaurant restaurant = restaurantLogic.GetRestaurantById(idRestaurant);
                if (restaurant == null) {
                    return NotFound();
                }
                return Ok(restaurant);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Route("api/Restaurant/Search/{nameSimilarRestaurant}")]
        [HttpGet]
        public IHttpActionResult GetBySimilarName(string nameSimilarRestaurant)
        {
            try
            {
                IEnumerable<Restaurant> restaurants = restaurantLogic.GetAllRestaurantsWithSimilarNameTo(nameSimilarRestaurant);
                if (restaurants == null)
                {
                    return NotFound();
                }
                return Ok(restaurants);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Route("api/Restaurant/Category/{idCategory:int}")]
        [HttpGet]
        public IHttpActionResult GetByCategory(int idCategory)
        {
            try
            {
                IEnumerable<Restaurant> restaurants = restaurantLogic.GetAllRestaurantsByCategoryId(idCategory);
                if(restaurants == null)
                {
                    return NotFound();
                }
                return Ok(restaurants);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Route("api/Restaurant/Competitor/{idCompetitor}")]
        [HttpGet]
        public IHttpActionResult GetRestaurantCompetitor(int idCompetitor)
        {
            try
            {
                List <RestaurantCompetitionFromApi> restaurantsCompetitionFromApi = restaurantLogic.GetCompetitors(idCompetitor);                
                if (restaurantsCompetitionFromApi == null)
                {
                    return NotFound();
                }
                return Ok(restaurantsCompetitionFromApi);
            }
            catch (RestaurantNotFoundException e)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, e.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        // POST: api/Restaurant
        [HttpPost]
        public IHttpActionResult Post([FromBody]Restaurant restaurant)
        {
            try
            {  
                if(!restaurantLogic.CategoriesAreCorrect(restaurant.Categories))
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se ha encontrado las categorias ingresadas"));
                }
                if (restaurantLogic.IsRestaurantNameRepeat(restaurant.Name)) {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Ambiguous, "No se puede crear un restaurante con un nombre ya existente"));
                }
                restaurantLogic.CreateRestaurant(restaurant);
                return Ok();     
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // PUT: api/Restaurant/5
        [HttpPut]
        public IHttpActionResult Put(int idRestaurant, [FromBody]Restaurant restaurantModified)
        {
            try
            {
                if (!restaurantLogic.CategoriesAreCorrect(restaurantModified.Categories))
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se ha encontrado las categorias ingresadas en el request"));
                }
                if (restaurantLogic.IsRestaurantNameRepeat(restaurantModified.Name, idRestaurant))
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Ambiguous, "No se puede crear un restaurante con un nombre ya existente"));
                }
                restaurantLogic.UpdateRestaurant(idRestaurant,restaurantModified);
                return Ok();
            }
            catch (RestaurantNotFoundException e)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, e.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // DELETE: api/Restaurant/5
        [HttpDelete]
        public IHttpActionResult Delete(int idRestaurant)
        {
            try
            {
                restaurantLogic.DeleteRestaurant(idRestaurant);
                return Ok();
            }
            catch (RestaurantNotFoundException e)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, e.Message  ));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
