using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestIT.Models
{
    public class RestaurantChef
    {
        public int RestaurantID { get; set; }
        public int ChefID { get; set; }

        //Relationships
        public virtual Restaurant Restaurent { get; set; }
        public virtual Chef Chef { get; set; }
    }
}
