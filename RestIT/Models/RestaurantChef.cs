using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestIT.Models
{
    public class RestaurantChef
    {
        public int ID { get; set; }
        public Restaurant restaurent { get; set; }
        public Chef chef { get; set; }
    }
}
