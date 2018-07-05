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
    [Route("api/Dishes")]
    public class DishesController : Controller
    {
        private readonly RestaurantContext _context;

        public DishesController(RestaurantContext context)
        {
            _context = context;
        }

        // GET: api/Dishes
        /// <summary>
        /// This method gets the dishes from the database.
        /// </summary>
        /// <returns>List of dishes.</returns>
        [HttpGet]
        public IEnumerable<Dish> GetDishes()
        {
            var x = _context.Dishes.Join(_context.Restaurants,
                    d => d.RestaurantId,
                    r => r.Id,
                    (d, r) => new { dish = d, rest = r }
                ).Select(selectResult => new Dish
                {
                    Id = selectResult.dish.Id,
                    Name = selectResult.dish.Name,
                    Price = selectResult.dish.Price,
                    RestaurantId = selectResult.rest.Id,
                    Restaurant = new Restaurant { Name = selectResult.rest.Name }
                }
                );
            return x;
        }

        // GET: api/Dishes/5
        /// <summary>
        /// This method gets the dishes specified
        /// </summary>
        /// <param name="id">Dishes Id</param>
        /// <returns>Dishes specified</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDish([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dish = await _context.Dishes.SingleOrDefaultAsync(m => m.Id == id);

            if (dish == null)
            {
                return NotFound();
            }

            return Ok(dish);
        }

        // PUT: api/Dishes/5
        /// <summary>
        /// This method update the dishes
        /// </summary>
        /// <param name="id">Dishes Id</param>
        /// <returns>The action results.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDish([FromRoute] Guid id, [FromBody] Dish dish)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != dish.Id)
            {
                return BadRequest();
            }

            _context.Entry(dish).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DishExists(id))
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

        // POST: api/Dishes
        /// <summary>
        /// This method created one dish
        /// </summary>
        /// <param name="dish">Object of type Dish that contains the data to save</param>
        /// <returns>The action results.</returns>
        [HttpPost]
        public async Task<IActionResult> PostDish([FromBody] Dish dish)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            _context.Dishes.Add(dish);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDish", new { id = dish.Id }, dish);
        }

        // DELETE: api/Dishes/5
        /// <summary>
        /// This method delete the dish
        /// </summary>
        /// <param name="id">Dish Id</param>
        /// <returns>The action results.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDish([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dish = await _context.Dishes.SingleOrDefaultAsync(m => m.Id == id);
            if (dish == null)
            {
                return NotFound();
            }

            _context.Dishes.Remove(dish);
            await _context.SaveChangesAsync();

            return Ok(dish);
        }

        private bool DishExists(Guid id)
        {
            return _context.Dishes.Any(e => e.Id == id);
        }
    }
}