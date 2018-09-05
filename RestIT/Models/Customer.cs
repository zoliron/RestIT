using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace RestIT.Models
{
    public class Customer
    {
        public int ID { get; set; }
        [DisplayName("Customer Name")]
        public String custName { get; set; }
        [DisplayName("Customer Phone")]
        public String custPhone { get; set; }
        [DisplayName("Customer Mail")]
        public String custMail { get; set; }
        [DisplayName("Customer Name")]
        public int custAge { get; set; }
    }
}
