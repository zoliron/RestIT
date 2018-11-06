using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestIT.Models
{
    public class TypeCalc
    {
        public int ID { get; set; }
        [DisplayName("Gender")]
        public Sex sex { get; set; }
        [RegularExpression("^[0-9]+$", ErrorMessage = "Age must be a number!")]

        [DisplayName("Age")]
        public float age { get; set; }
        [DisplayName("City")]

        public City city { get; set; }
        public string Type { get; set;  }   

    }
   
}
