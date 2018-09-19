using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RestIT.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<RestIT.Models.Chef> Chef { get; set; }

        public DbSet<RestIT.Models.Customer> Customer { get; set; }

        public DbSet<RestIT.Models.Dish> Dish { get; set; }

        public DbSet<RestIT.Models.Restaurant> Restaurant { get; set; }
    }
}
