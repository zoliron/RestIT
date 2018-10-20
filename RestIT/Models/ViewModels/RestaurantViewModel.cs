using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestIT.Models.ViewModels
{
    public class RestaurantViewModel : Restaurant
    {
        private Restaurant restaurant;

        public RestaurantViewModel(Restaurant restaurant)
        {
            this.restaurant = restaurant;
        }
    }
}
