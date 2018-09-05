﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestIT.Models
{
    public class Dish
    {
        public int ID { get; set; }
        [DisplayName("Dish Name")]
        public String dishName { get; set; }
        [DisplayName("Dish Cost")]
        public int dishCost { get; set; }
        [Required(ErrorMessage = "Please enter your rate")]
        [Range(0, 5, ErrorMessage = "Enter number between 0 to 5")]
        [DisplayName("Dish Rating")]
        public float dishRating { get; set; }
        [DisplayName("Dish Type")]
        public String dishType { get; set; }
    }
}
