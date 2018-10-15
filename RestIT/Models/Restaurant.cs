using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestIT.Models
{
    public class Restaurant
    {
        public int ID { get; set; }
        [DisplayName("Restuarant Name")]
        public String restName { get; set; }
        [DisplayName("Restuarant Location")]
        public String restLocation { get; set; }
        [Required(ErrorMessage = "Please enter your rate")]
        [Range(0, 5, ErrorMessage = "Enter number between 0 to 5")]
        [DisplayName("Restuarant Rating")]
        public float restRating { get; set; }
        public virtual ICollection<Dish> Dishes { get; set; }
        [DisplayName("Restuarant Type")]
        public String restType { get; set; }
        [DisplayName("Kosher")]
        public Boolean restKosher { get; set; }

    }
}
