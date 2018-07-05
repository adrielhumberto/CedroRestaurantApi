using System;
using System.Collections.Generic;

namespace CedroRestaurantApi.Entities
{
    public class Restaurant
    {

        public Guid Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Dish> Dishes { get; set; }
    }
}
