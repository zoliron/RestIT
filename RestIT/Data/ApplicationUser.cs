﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestIT.Data
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string userRole { get; set; }
    }
}