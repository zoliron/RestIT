using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestIT.Models
{
    public class Chef
    {
        public int ID { get; set; }
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$" , ErrorMessage = "Must enter a valid name! (Capital letter first)")]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 30 characters.")]
        [DisplayName("Chef Name")]
        public String chefName { get; set; }
        public virtual ICollection<Restaurant> Restuarants { get; set; }
    }
}