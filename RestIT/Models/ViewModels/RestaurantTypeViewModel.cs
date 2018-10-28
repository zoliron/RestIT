using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestIT.Models.ViewModels
{
    public class RestaurantTypeViewModel
    {
        public List<Restaurant> Restaurants;
        public SelectList Types;
        public string RestaurantType { get; set; }
        public string SearchString { get; set; }
    }
}
