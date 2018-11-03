using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestIT.Models
{
    public enum RestType : int
    {
        None=0,
        Italian,
        Israeli,
        Meat,
        Mediterranean,
        Europe,
        Fishes,
        Homemade,
        Desserts,
        Mexican,
        Asian,

        

    }; 
    public class Restaurant
    {
        public int ID { get; set; }
        [DisplayName("Restuarant's chef")]
        [Required]
        public virtual ICollection<RestaurantChef> restChef { get; set; }
        [DisplayName("Restuarant Name")]
        public String restName { get; set; }
        [DisplayName("Restuarant Address")]
        public String restAddress { get; set; }
        [DisplayName("Restuarant City")]
        public String restCity { get; set; }
        [Required(ErrorMessage = "Please enter your rate")]
        [Range(0, 5, ErrorMessage = "Enter number between 0 to 5")]
        [DisplayName("Restuarant Rating")]
        public double restRating { get; set; }
        [DisplayName("Restuarant's Dishes")]
        public virtual ICollection<Dish> Dishes { get; set; }
        [DisplayName("Restuarant Type")]
        public RestType restType { get; set; }
        [DisplayName("Kosher")]
        public Boolean restKosher { get; set; }
        public double restLat{ get; set; }
        public double restLng { get; set; }
    }
}
