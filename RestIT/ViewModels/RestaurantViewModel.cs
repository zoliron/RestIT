using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestIT.Models;

namespace RestIT.ViewModels
{
    public class RestaurantViewModel : Restaurant
    {
        // This is a list of all chef's IDs that were selected in the view
        public List<int> SelectedChef { get; set; }
        public List<Chef> Chefs { get; set; }

        public RestaurantViewModel() { }

        public RestaurantViewModel(Restaurant restaurant, List<Chef> Chefs)
        {
            this.restKosher = restaurant.restKosher;
            this.restAddress = restaurant.restAddress;
            this.restCity = restaurant.restCity;
            this.restName = restaurant.restName;
            this.restRating = restaurant.restRating;
            this.restType = restaurant.restType;
            this.Dishes = new List<Dish>();
            this.ID = restaurant.ID;
            this.Chefs = Chefs;

            // Populate selectedChef with as many elements as in chefs list
            SelectedChef = new List<int>();
            foreach (Chef chef in Chefs)
            {
                int cid = chef.ID;
                SelectedChef.Add(cid);
            }
        }
        public RestaurantViewModel(Restaurant restaurant)
        {
            this.restKosher = restaurant.restKosher;
            this.restAddress = restaurant.restAddress;
            this.restCity = restaurant.restCity;
            this.restName = restaurant.restName;
            this.restRating = restaurant.restRating;
            this.restType = restaurant.restType;
            this.Dishes = new List<Dish>();
            this.ID = restaurant.ID;
        }
    }
}
