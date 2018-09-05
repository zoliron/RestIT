using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RestIT.Models;

namespace RestIT.Models
{
    public class RestITContext : DbContext
    {
        public RestITContext (DbContextOptions<RestITContext> options)
            : base(options)
        {
        }

        public DbSet<RestIT.Models.Chef> Chef { get; set; }

        public DbSet<RestIT.Models.Customer> Customer { get; set; }

        public DbSet<RestIT.Models.Dish> Dish { get; set; }

        public DbSet<RestIT.Models.Restaurant> Restaurant { get; set; }
    }
}
