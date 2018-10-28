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
                SeedDishDB(context, adminID);
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
                    restAddress = "Nissim Aloni 10",
                    restCity = "Tel Aviv",
                    restRating = 4,
                    restType = "Prestige",
                    restKosher = true,
                    Lat= 32.089348,
                    Lng= 34.797386,
                },
                new Restaurant
                {
                    restName = "Yaffo Tel Aviv",
                    restAddress = "Yigal Alon 98",
                    restCity = "Tel Aviv",
                    restRating = 3,
                    restType = "Mediterranean",
                    restKosher = true,
                    Lat=32.070353,
                    Lng=34.794209,
                },
                new Restaurant
                {
                    restName = "Mashya",
                    restAddress = "5 Mendeli st.",
                    restCity = "Tel Aviv",
                    restRating = 4,
                    restType = "Fishes, Meat",
                    restKosher = false,
                    Lat= 32.079671,
                    Lng= 34.768773,
                },
                new Restaurant
                {
                    restName = "MachneYuda",
                    restAddress = "Beit Jacob 10",
                    restCity = "Jerusalem",
                    restRating = 3,
                    restType = "Mediterranean",
                    restKosher = false,
                    Lat = 31.785449,
                    Lng = 35.211158,
                },
                new Restaurant
                {
                    restName = "Claro",
                    restAddress = "Ha''arbaa st 23",
                    restCity = "Tel Aviv",
                    restRating = 4,
                    restType = "Fishes, Meat",
                    restKosher = false,
                    Lat=32.070930,
                    Lng=34.787801,
                },
                new Restaurant
                {
                    restName = "Taizu",
                    restAddress = "Menahim Begin 23",
                    restCity = "Tel Aviv",
                    restRating = 5,
                    restType = "Asian",
                    restKosher = false,
                    Lat=32.063943,
                    Lng=34.780015,
                },
                new Restaurant
                {
                    restName = "Pronto",
                    restAddress = "Hertzel 6",
                    restCity = "Tel Aviv",
                    restRating = 4,
                    restType = "Italian",
                    restKosher = false,
                    Lat=32.063641,
                    Lng=34.769784,
                },
                new Restaurant
                {
                    restName = "Girrafe",
                    restAddress = "Ibn Gabirol 49",
                    restCity = "Tel Aviv",
                    restRating = 3,
                    restType = "Asian",
                    restKosher = false,
                    Lat=32.077274,
                    Lng=34.781233,
                },
                new Restaurant
                {
                    restName = "The Old Lady",
                    restAddress = "Amiad 14",
                    restCity = "Tel Aviv",
                    restRating = 5,
                    restType = "Pizza",
                    restKosher = false,
                    Lat=32.053589,
                    Lng=34.755772,
                },
                new Restaurant
                {
                    restName = "Rustico",
                    restAddress = "Rothschild blvd. 15",
                    restCity = "Tel Aviv",
                    restRating = 4.5,
                    restType = "Italian",
                    restKosher = true,
                    Lat=32.063546,
                    Lng=34.770959,
                },
                new Restaurant
                {
                    restName = "2C",
                    restAddress = "Menachem Begin 132",
                    restCity = "Tel Aviv",
                    restRating = 5,
                    restType = "Prestige, Mediterranean",
                    restKosher = true,
                    Lat = 32.074774,
                    Lng= 34.791573,
                },
                new Restaurant
                {
                    restName = "Alabama",
                    restAddress = "Pinkas David 47",
                    restCity = "Netanya",
                    restRating = 5,
                    restType = "Meat",
                    restKosher = false,
                    Lat=32.314429,
                    Lng=34.873948,
                },
                new Restaurant
                {
                    restName = "Limousine",
                    restAddress = "Horesh HaAlonim St 14",
                    restCity = "Ramat Yishai",
                    restRating = 4,
                    restType = "Meat",
                    restKosher = false,
                    Lat= 32.709285,
                    Lng= 35.174259,
                },
                new Restaurant
                {
                    restName = "Whale",
                    restAddress = "Ha-Yam St. 6",
                    restCity = "Eilat",
                    restRating = 4,
                    restType = "French, Mediterranean,  Fishes",
                    restKosher = false,
                    Lat= 29.548329,
                    Lng= 34.967564,
                },
                new Restaurant
                {
                    restName = "Santa Katarina",
                    restAddress = "Har Sinai 2",
                    restCity = "Tel Aviv",
                    restRating = 4,
                    restType = "Chef Restaurant",
                    restKosher = false,
                    Lat= 32.064640,
                    Lng= 34.771773,
                },
                new Restaurant
                {
                    restName = "Ouzeria",
                    restAddress = "Metalon 44",
                    restCity = "Tel Aviv",
                    restRating = 3,
                    restType = "Mediterranean, Fishes",
                    restKosher = false,
                    Lat= 32.059046,
                    Lng= 34.772716,
                },
                new Restaurant
                {
                    restName = "Dallal",
                    restAddress = "Shabazi 10",
                    restCity = "Tel Aviv",
                    restRating = 5,
                    restType = "Mediterranean, Europe, Fishes, Pasta",
                    restKosher = false,
                    Lat=32.061327,
                    Lng= 34.763459,
                },
                new Restaurant
                {
                    restName = "Suzana",
                    restAddress = "Shabazi 9",
                    restCity = "Tel Aviv",
                    restRating = 4,
                    restType = "Mediterranean, Meat, Homemade food, Fishes, Stews, Breakfest ",
                    restKosher = false,
                    Lat= 32.061646,
                    Lng= 34.763607,
                },
                new Restaurant
                {
                    restName = "Golda",
                    restAddress = "Mikve Israel 18",
                    restCity = "Tel Aviv",
                    restRating = 5,
                    restType = "Ice cream parlor, Desserts",
                    restKosher = false,
                    Lat= 32.062874,
                    Lng= 34.776297,
                },
                new Restaurant
                {
                    restName = "Aldo",
                    restAddress = "Allenby 1",
                    restCity = "Tel Aviv",
                    restRating = 4,
                    restType = "Ice cream parlor, Desserts",
                    restKosher = false,
                    Lat=32.069460,
                    Lng=34.783678,
                },
                new Restaurant
                {
                    restName = "Taqueria",
                    restAddress = "Levontin 28",
                    restCity = "Tel Aviv",
                    restRating = 5,
                    restType = "Mexican",
                    restKosher = false,
                    Lat=32.063193,
                    Lng=34.776675,
                },
                new Restaurant
                {
                    restName = "Jericho",
                    restAddress = "Pinchas Ben Yair 4",
                    restCity = "Tel Aviv",
                    restRating = 5,
                    restType = "Mediterranean,  Fishes, Meat",
                    restKosher = false,
                    Lat=32.090798,
                    Lng=34.781022,
                });
            context.SaveChanges();
        }
        #endregion

        #region SeedDishDB
        public static void SeedDishDB(ApplicationDbContext context, string adminID)
        {
            if (context.Dish.Any())
            {
                return;   // DB has been seeded
            }

            context.Dish.AddRange(
                new Dish
                {
                    dishName = "Cheese stuffed mushrooms",
                    dishCost = 100,
                    dishRating = 4,
                    dishType = "First dish",
                    dishIngredients = "Mushrooms, Cheese, Garlic, Coriander, Cream",
                    dishImage = null
                },
                new Dish
                {
                    dishName = "Fish & Chips",
                    dishCost = 60,
                    dishRating = 3,
                    dishType = "First dish",
                    dishIngredients = "Bass fish, Potatoes, Coriander, Garlic, Onions, Carrot",
                    dishImage = null
                },
                new Dish
                {
                    dishName = "Carpaccio",
                    dishCost = 60,
                    dishRating = 5,
                    dishType = "First dish",
                    dishIngredients = "Beef tenderloin, Arugala, Vinaigrette sauce, Parmezan",
                    dishImage = null
                },
                new Dish
                {
                    dishName = "Chicken salad",
                    dishCost = 54,
                    dishRating = 4,
                    dishType = "First dish",
                    dishIngredients = "Chicken breast, Tomatoes, Lettuce, Cucumbers, Red peppers, Onions, Vinaigrette sauce",
                    dishImage = null
                },
                new Dish
                {
                    dishName = "Thai Style Sweet Chili",
                    dishCost = 59,
                    dishRating = 3,
                    dishType = "First dish",
                    dishIngredients = "Sweet chili sauce, Chicken wings, Parsley",
                    dishImage = null
                },
                new Dish
                {
                    dishName = "Sushi tempura",
                    dishCost = 56,
                    dishRating = 4,
                    dishType = "First dish",
                    dishIngredients = "Rice, Salmon fish, Avocado, Cream cheese, Tamago, Panko bread crumbs",
                    dishImage = null
                },
                new Dish
                {
                    dishName = "Greek Salad",
                    dishCost = 49,
                    dishRating = 4,
                    dishType = "First dish",
                    dishIngredients = "Lettuce, Cucumbers, Red peppers, Onions, Tomatoes, Feta cheese, Fried mushrooms",
                    dishImage = null
                },
                new Dish
                {
                    dishName = "Chicken breast",
                    dishCost = 75,
                    dishRating = 5,
                    dishType = "Main dish",
                    dishIngredients = "Chicken breast, Peppers, Salt, Sweet potato fries, BBQ sauce",
                    dishImage = null
                },
                new Dish
                {
                    dishName = "Spaghetti alfredo",
                    dishCost = 69,
                    dishRating = 4,
                    dishType = "Main dish",
                    dishIngredients = "Mushrooms, Sour cream, Onions, Black pepper, Parmesan cheese",
                    dishImage = null
                },
                new Dish
                {
                    dishName = "Spaghetti bolognese",
                    dishCost = 69,
                    dishRating = 5,
                    dishType = "Main dish",
                    dishIngredients = "Beef, Tomatoes sauce, Onions, Carrots, Coriander, Paprika",
                    dishImage = null
                },
                new Dish
                {
                    dishName = "chicken curry",
                    dishCost = 75,
                    dishRating = 5,
                    dishType = "Main dish",
                    dishIngredients = "Green curry, Cabbage, Sprouts, Peas, coconut milk",
                    dishImage = null
                },
                new Dish
                {
                    dishName = "Pad thai",
                    dishCost = 75,
                    dishRating = 4,
                    dishType = "Main dish",
                    dishIngredients = "Sprouts, Carrot, Mushrooms, Cabbage, Red pepper",
                    dishImage = null
                },
                new Dish
                {
                    dishName = "Hamburger",
                    dishCost = 84,
                    dishRating = 5,
                    dishType = "Main dish",
                    dishIngredients = "Beef, Tomatoes, Onions, Lettuce, Pickles, Mayonnaise",
                    dishImage = null
                },
                new Dish
                {
                    dishName = "Salmon fillet",
                    dishCost = 110,
                    dishRating = 5,
                    dishType = "Main dish",
                    dishIngredients = "Salmon, Lemon juice, Parsley, English black pepper, Olive oil",
                    dishImage = null
                },
                new Dish
                {
                    dishName = "Schnitzel with fries",
                    dishCost = 89,
                    dishRating = 5,
                    dishType = "Main dish",
                    dishIngredients = "Chicken schnitzel, Bread crumbs, Potatoes",
                    dishImage = null
                },
                new Dish
                {
                    dishName = "Entrecote",
                    dishCost = 120,
                    dishRating = 5,
                    dishType = "Main dish",
                    dishIngredients = "Beef entrecote",
                    dishImage = null
                },
                new Dish
                {
                    dishName = "Chocolate souffle",
                    dishCost = 39,
                    dishRating = 5,
                    dishType = "Desert",
                    dishIngredients = "Heavy cream, Dark chocolate, Milk, Flour",
                    dishImage = null
                },
                new Dish
                {
                    dishName = "Creme brulee",
                    dishCost = 39,
                    dishRating = 5,
                    dishType = "Desert",
                    dishIngredients = "Brown sugar, Heavy cream, Eggs, Bananas",
                    dishImage = null
                },
                new Dish
                {
                    dishName = "Cheesecake with cranberries",
                    dishCost = 39,
                    dishRating = 5,
                    dishType = "Desert",
                    dishIngredients = "Cracker crumbs, Cream cheese, Eggs, Vanilla, Cranberries sauce",
                    dishImage = null
                },
                new Dish
                {
                    dishName = "Pavlova",
                    dishCost = 39,
                    dishRating = 5,
                    dishType = "Desert",
                    dishIngredients = "Whipped cream , Fresh strawberries, Eggs",
                    dishImage = null
                },
                new Dish
                {
                    dishName = "Coca Cola",
                    dishCost = 14,
                    dishRating = 0,
                    dishType = "Beverage",
                    dishIngredients = null,
                    dishImage = null
                },
                new Dish
                {
                    dishName = "Water",
                    dishCost = 10,
                    dishRating = 0,
                    dishType = "Beverage",
                    dishIngredients = null,
                    dishImage = null
                },
                new Dish
                {
                    dishName = "Orange juice",
                    dishCost = 17,
                    dishRating = 0,
                    dishType = "Beverage",
                    dishIngredients = null,
                    dishImage = null
                },
                new Dish
                {
                    dishName = "Sprite",
                    dishCost = 14,
                    dishRating = 0,
                    dishType = "Beverage",
                    dishIngredients = null,
                    dishImage = null
                });
            context.SaveChanges();
        } 
        #endregion
    }
}
