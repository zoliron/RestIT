using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestIT.Models
{
    public class Customer : IdentityUser
    {
        // user ID from AspNetUser table.
        public string OwnerID { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$", ErrorMessage = "Must enter a valid name! (Capital letter first)")]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 30 characters.")]
        [DisplayName("Customer Name")]
        public String custName { get; set; }

        [RegularExpression("^[0-9]+$", ErrorMessage = "Phone must be a number!")]
        [DisplayName("Customer Phone")]
        public String custPhone { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [DisplayName("Customer Mail")]
        public String custMail { get; set; }

        [DisplayName("Customer Age")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Age must be a number!")]
        public int custAge { get; set; }

        [DisplayName("Favourite Restuarant Type")]
        public CustomerRestType custRestType { get; set; }

        public CustomerStatus Status { get; set; }
    }

    public enum CustomerStatus
    {
        Submitted,
        Approved,
        Rejected
    }

    public enum CustomerRestType : int
    {
        None = 0,
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
}
