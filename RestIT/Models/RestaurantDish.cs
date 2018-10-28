using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestIT.Models
{
    public class RestaurantDish
    {
        public int ID { get; set; }
        public Restaurant restaurent { get; set; }
        public Dish dish { get; set; }
    }
}
