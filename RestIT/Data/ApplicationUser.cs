using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestIT.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string userGender { get; set; }
        public string userAge { get; set; }
        public string userCity { get; set; }
        public string userFavouriteRestType { get; set; }
    }
}
