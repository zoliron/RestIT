using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestIT.Models.ViewModels
{
    public class RestaurantSearchViewModel
    {
        public List<Restaurant> Restaurants;
        public SelectList Types;
        public SelectList Locations;
        public SelectList Raitings;
        public string RestaurantType { get; set; }
        public string RestaurantLocation { get; set; }
        public int RestaurantRating { get; set; }
        public string SearchString { get; set; }
    }
}
