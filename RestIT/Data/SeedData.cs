using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RestIT.Areas.Identity.Authorization;
using RestIT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestIT.Data
{
    public class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, string testUserPw)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // For sample purposes seed both with the same password.
                // Password is set with the following:
                // dotnet user-secrets set SeedUserPW <pw>
                // The admin user can do anything

                var adminID = await EnsureUser(serviceProvider, testUserPw, "admin@restit.com");
                await EnsureRole(serviceProvider, adminID, Constants.CustomerAdministratorsRole);

                // allowed user can create and edit contacts that they create
                var managerID = await EnsureUser(serviceProvider, testUserPw, "manager@restit.com");
                await EnsureRole(serviceProvider, managerID, Constants.CustomerManagersRole);

                SeedCustomerDB(context, adminID);
                SeedChefDB(context, adminID);
                SeedRestaurantDB(context, adminID);
            }
        }

        private static async Task<string> EnsureUser(IServiceProvider serviceProvider,
                                                    string testUserPw, string UserName)
        {
            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();

            var user = await userManager.FindByNameAsync(UserName);
            if (user == null)
            {
                user = new ApplicationUser { UserName = UserName };
                await userManager.CreateAsync(user, testUserPw);
            }

            return user.Id;
        }

        private static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider,
                                                                      string uid, string role)
        {
            IdentityResult IR = null;
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            if (roleManager == null)
            {
                throw new Exception("roleManager null");
            }

            if (!await roleManager.RoleExistsAsync(role))
            {
                IR = await roleManager.CreateAsync(new IdentityRole(role));
            }

            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();

            var user = await userManager.FindByIdAsync(uid);

            IR = await userManager.AddToRoleAsync(user, role);

            return IR;
        }

        #region SeedCustomerDB
        public static void SeedCustomerDB(ApplicationDbContext context, string adminID)
        {
            if (context.Customer.Any())
            {
                return;   // DB has been seeded
            }

            context.Customer.AddRange(
                new Customer
                {
                    custName = "Ronen Zolicha",
                    custAge = 25,
                    custPhone = "0547713375",
                    custMail = "ronen1245@gmail.com",
                    Status = CustomerStatus.Approved,
                    OwnerID = adminID
                });
            context.SaveChanges();
        } 
        #endregion

        #region SeedChefDB
        public static void SeedChefDB(ApplicationDbContext context, string adminID)
        {
            if (context.Chef.Any())
            {
                return;   // DB has been seeded
            }
            context.Chef.AddRange(
                new Chef
                {
                    chefName = "Eyal Shani"
                },
                new Chef
                {
                    chefName = "Haim Cohen"
                },
                new Chef
                {
                    chefName = "Shaul Ben Aderet"
                },
                new Chef
                {
                    chefName = "Yossi Shitrit"
                },
                new Chef
                {
                    chefName = "Assaf Granit"
                },
                new Chef
                {
                    chefName = "Ran Shmueli"
                },
                new Chef
                {
                    chefName = "Yuval Ben Neriah"
                },
                new Chef
                {
                    chefName = "David Frenkel"
                },
                new Chef
                {
                    chefName = "Tomer Agai"
                },
                new Chef
                {
                    chefName = "Avivit Priel Avichai"
                },
                new Chef
                {
                    chefName = "Dana Yarzin"
                });
            context.SaveChanges();
        }
        #endregion

        #region SeedRestaurantDB
        public static void SeedRestaurantDB(ApplicationDbContext context, string adminID)
        {
            if (context.Restaurant.Any())
            {
                return;   // DB has been seeded
            }

            context.Restaurant.AddRange(
                new Restaurant
                {
                    restName = "The Blue Rooster",
                    restLocation = "Nissim Aloni 10, Tel Aviv",
                    restRating = 4,
                    restType = "Prestige",
                    restKosher = true,
                },
                new Restaurant
                {
                    restName = "Yaffo Tel Aviv",
                    restLocation = "Yigal Alon 98, Tel Aviv",
                    restRating = 3,
                    restType = "Mediterranean",
                    restKosher = true,
                },
                new Restaurant
                {
                    restName = "Mashya",
                    restLocation = "5 Mendeli st., Tel Aviv",
                    restRating = 4,
                    restType = "Fishes, Meat",
                    restKosher = false,
                },
                new Restaurant
                {
                    restName = "MachneYuda",
                    restLocation = "Beit Jacob 10, Jerusalem",
                    restRating = 3,
                    restType = "Mediterranean",
                    restKosher = false,
                },
                new Restaurant
                {
                    restName = "Claro",
                    restLocation = "Ha''arbaa st 23, Tel Aviv",
                    restRating = 4,
                    restType = "Fishes, Meat",
                    restKosher = false,
                },
                new Restaurant
                {
                    restName = "Taizu",
                    restLocation = "Menahim Begin 23, Tel Aviv",
                    restRating = 5,
                    restType = "Asian",
                    restKosher = false,
                },
                new Restaurant
                {
                    restName = "Pronto",
                    restLocation = "Hertzel 6, Tel Aviv",
                    restRating = 4,
                    restType = "Italian",
                    restKosher = false,
                },
                new Restaurant
                {
                    restName = "Girrafe",
                    restLocation = "Ibn Gabirol 49, Tel Aviv",
                    restRating = 3,
                    restType = "Asian",
                    restKosher = false,
                },
                new Restaurant
                {
                    restName = "The Old Lady",
                    restLocation = "Amiad 14, Tel Aviv",
                    restRating = 5,
                    restType = "Pizza",
                    restKosher = false,
                },
                new Restaurant
                {
                    restName = "Rustico",
                    restLocation = "Rothschild blvd. 15, Tel Aviv",
                    restRating = 4.5,
                    restType = "Italian",
                    restKosher = true,
                },
                new Restaurant
                {
                    restName = "2C",
                    restLocation = "Menachem Begin 132, Tel Aviv",
                    restRating = 5,
                    restType = "Prestige, Mediterranean",
                    restKosher = true,
                },
                new Restaurant
                {
                    restName = "Alabama",
                    restLocation = "Pinkas David 47, Netanya",
                    restRating = 5,
                    restType = "Meat",
                    restKosher = false,
                },
                new Restaurant
                {
                    restName = "Limousine",
                    restLocation = "Horesh HaAlonim St 14, Ramat Yishai",
                    restRating = 4,
                    restType = "Meat",
                    restKosher = false,
                },
                new Restaurant
                {
                    restName = "Whale",
                    restLocation = "Ha-Yam St. 6, Eilat",
                    restRating = 4,
                    restType = "French, Mediterranean,  Fishes",
                    restKosher = false,
                },
                new Restaurant
                {
                    restName = "Santa Katarina",
                    restLocation = "Har Sinai 2, Tel Aviv",
                    restRating = 4,
                    restType = "Chef Restaurant",
                    restKosher = false,
                },
                new Restaurant
                {
                    restName = "Ouzeria",
                    restLocation = "Metalon 44, Tel Aviv",
                    restRating = 3,
                    restType = "Mediterranean, Fishes",
                    restKosher = false,
                },
                new Restaurant
                {
                    restName = "Dallal",
                    restLocation = "Shabazi 10, Tel Aviv",
                    restRating = 5,
                    restType = "Mediterranean, Europe, Fishes, Pasta",
                    restKosher = false,
                },
                new Restaurant
                {
                    restName = "Suzana",
                    restLocation = "Shabazi 9, Tel Aviv",
                    restRating = 4,
                    restType = "Mediterranean, Meat, Homemade food, Fishes, Stews, Breakfest ",
                    restKosher = false,
                },
                new Restaurant
                {
                    restName = "Golda",
                    restLocation = "Mikve Israel 18, Tel Aviv",
                    restRating = 5,
                    restType = "Ice cream parlor, Desserts",
                    restKosher = false,
                },
                new Restaurant
                {
                    restName = "Aldo",
                    restLocation = "Allenby 1, Tel Aviv",
                    restRating = 4,
                    restType = "Ice cream parlor, Desserts",
                    restKosher = false,
                },
                new Restaurant
                {
                    restName = "Taqueria",
                    restLocation = "Levontin 28, Tel Aviv",
                    restRating = 5,
                    restType = "Mexican",
                    restKosher = false,
                },
                new Restaurant
                {
                    restName = "Jericho",
                    restLocation = "Pinchas Ben Yair 4, Tel Aviv",
                    restRating = 5,
                    restType = "Mediterranean,  Fishes, Meat",
                    restKosher = false,
                });
            context.SaveChanges();
        } 
        #endregion
    }
}
