using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestIT.Models.ViewModels
{
    public class RestaurantViewModel : Restaurant
    {
        public RestaurantViewModel() { }
        public RestaurantViewModel(Restaurant rest)
        {
            this.restKosher = rest.restKosher;
            this.restAddress = rest.restAddress;
            this.restCity = rest.restCity;
            this.restName = rest.restName;
            this.restRating = rest.restRating;
            this.restType = rest.restType;

            this.Dishes = new List<Dish>();

        }
    }
}
