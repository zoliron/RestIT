using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RestIT.Models;

namespace RestIT.Data
{
    public class ApplicationDbContext : IdentityDbContext<Customer>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<RestIT.Models.Chef> Chef { get; set; }

        public DbSet<RestIT.Models.Customer> Customer { get; set; }

        public DbSet<RestIT.Models.Dish> Dish { get; set; }

        public DbSet<RestIT.Models.Restaurant> Restaurant { get; set; }

        public DbSet<RestIT.Models.RestaurantChef> RestaurantChef { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RestaurantChef>()
                .HasKey(pt => new { pt.RestaurantID, pt.ChefID });

            modelBuilder.Entity<RestaurantChef>()
                .HasOne(pt => pt.Restaurent)
                .WithMany(p => p.restChef)
                .HasForeignKey(pt => pt.RestaurantID);

            modelBuilder.Entity<RestaurantChef>()
                .HasOne(pt => pt.Chef)
                .WithMany(t => t.Restuarants)
                .HasForeignKey(pt => pt.ChefID);
        }
    }
}
