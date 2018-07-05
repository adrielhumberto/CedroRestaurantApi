using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CedroRestaurantApi.Context;
using CedroRestaurantApi.Entities;

namespace CedroRestaurantApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Restaurants")]
    public class RestaurantsController : Controller
    {
        private readonly RestaurantContext _context;

        public RestaurantsController(RestaurantContext context)
        {
            _context = context;
        }

        // GET: api/Restaurants
        /// <summary>
        /// This method gets the restaurants from the database.
        /// </summary>
        /// <returns>List of restaurants.</returns>
        [HttpGet]
        public IEnumerable<Restaurant> GetRestaurants()
        {
            return _context.Restaurants;
        }


        // GET: api/Restaurants/5
        /// <summary>
        /// This method gets the restaurant specified
        /// </summary>
        /// <param name="id">Restaurant Id</param>
        /// <returns>Restaurant specified</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRestaurant([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var restaurant = await _context.Restaurants.SingleOrDefaultAsync(m => m.Id == id);

            if (restaurant == null)
            {
                return NotFound();
            }

            return Ok(restaurant);
        }

        // PUT: api/Restaurants/5
        /// <summary>
        /// This method update the restaurant
        /// </summary>
        /// <param name="id">Restaurant Id</param>
        /// <returns>The action results.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRestaurant([FromRoute] Guid id, [FromBody] Restaurant restaurant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != restaurant.Id)
            {
                return BadRequest();
            }

            _context.Entry(restaurant).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RestaurantExists(id))
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

        // POST: api/Restaurants
        /// <summary>
        /// This method created one restaurant
        /// </summary>
        /// <param name="restaurant">Object of type Restaurant that contains the data to save</param>
        /// <returns>The action results.</returns>
        [HttpPost]
        public async Task<IActionResult> PostRestaurant([FromBody] Restaurant restaurant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Restaurants.Add(restaurant);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRestaurant", new { id = restaurant.Id }, restaurant);
        }

        // DELETE: api/Restaurants/5
        /// <summary>
        /// This method delete the restaurant
        /// </summary>
        /// <param name="id">Restaurant Id</param>
        /// <returns>The action results.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRestaurant([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var restaurant = await _context.Restaurants.SingleOrDefaultAsync(m => m.Id == id);
            if (restaurant == null)
            {
                return NotFound();
            }

            _context.Restaurants.Remove(restaurant);
            await _context.SaveChangesAsync();

            return Ok(restaurant);
        }

        /// <summary>
        /// This method checks if the restaurant exists
        /// </summary>
        /// <param name="id">Restaurant Id</param>
        /// <returns>Boolean of the result of exists any restaurant.</returns>
        private bool RestaurantExists(Guid id)
        {
            return _context.Restaurants.Any(e => e.Id == id);
        }
    }
}