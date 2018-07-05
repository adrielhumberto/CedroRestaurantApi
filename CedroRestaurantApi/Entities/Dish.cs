using System;

namespace CedroRestaurantApi.Entities
{
    public class Dish
    {

        public Guid Id { get; set; }

        public string Name { get; set; }
        public decimal Price { get; set; }

        public Guid RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }
    }
}
