using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestIT.Models
{
    public class RestaurantDish
    {
        public int RestaurantID { get; set; }
        public int DishID { get; set; }

        public string dishName { get; set; }

        //Relationships
        public virtual Restaurant Restaurant { get; set; }
        public virtual Dish Dish { get; set; }
    }
}
