using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace RestIT.Models
{
    public class Chef
    {
        public int ID { get; set; }
        [DisplayName("Chef Name")]
        public String chefName { get; set; }
        public virtual ICollection<Restaurant> Restuarants { get; set; }
    }
}