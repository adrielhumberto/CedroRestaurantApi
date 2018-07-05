using CedroRestaurantApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace CedroRestaurantApi.Context
{
    public class RestaurantContext : DbContext
    {
        public RestaurantContext(DbContextOptions<RestaurantContext> options) : base(options) { }

        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        
    }
}
