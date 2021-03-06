﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestIT.Models.ViewModels
{
    public class DishSearchViewModel
    {
        public List<Dish> Dishes;
        public List<Chef> Chefs;
        public SelectList Types;
        public string DishType { get; set; }
        public string SearchString { get; set; }
        public string SearchChef { get; set; }
    }
}
