﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestIT.Models
{
    public class RestaurantChef
    {
        [Key]
        public int ID { get; set; }
        public int RestaurantID { get; set; }
        public int ChefID { get; set; }

        //Relationships
        public Restaurant Restaurent { get; set; }
        public Chef Chef { get; set; }
    }
}
