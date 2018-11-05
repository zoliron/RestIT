using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RestIT.Areas.Identity.Authorization;
using RestIT.Areas.Identity.Pages.Account;
using RestIT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

                // allowed user can create and edit customers that they create
                var managerID = await EnsureUser(serviceProvider, testUserPw, "manager@restit.com");
                await EnsureRole(serviceProvider, managerID, Constants.CustomerManagersRole);

                SeedCustomerDB(context, adminID);
                UpdateCustomersPasswords(context, "Tt123!@#");
                SeedChefDB(context, adminID);
                SeedRestaurantDB(context, adminID);
                SeedDishDB(context, adminID);
            }
        }

        private static async Task<string> EnsureUser(IServiceProvider serviceProvider,
                                                    string testUserPw, string UserName)
        {
            var userManager = serviceProvider.GetService<UserManager<Customer>>();

            var user = await userManager.FindByNameAsync(UserName);
            if (user == null)
            {
                user = new Customer { UserName = UserName };
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

            var userManager = serviceProvider.GetService<UserManager<Customer>>();

            var user = await userManager.FindByIdAsync(uid);

            IR = await userManager.AddToRoleAsync(user, role);

            return IR;
        }

        #region SeedCustomerDB
        public static void SeedCustomerDB(ApplicationDbContext context, string adminID)
        {
            //if (context.Customer.Any())
            //{
            //    return;   // DB has been seeded
            //}

            var customers = from m in context.Users
                            where m.UserName != "admin@restit.com" &&
                            m.UserName != "manager@restit.com" &&
                            m.UserName != null
                            select m;

            if (customers.Any())
            {
                return; // DB has been seeded
            }
            else
            {
                context.Customer.AddRange(
                    new Customer
                    {
                        custName = "TestUser00",
                        custAge = 31,
                        custPhone = "0501234500",
                        UserName = "TestUser00@gmail.com",
                        Status = CustomerStatus.Approved,
                        custRestType = CustomerRestType.Asian,
                        OwnerID = adminID
                    },
                    new Customer
                    {
                        custName = "TestUser01",
                        custAge = 26,
                        custPhone = "0501234501",
                        UserName = "TestUser01@gmail.com",
                        Status = CustomerStatus.Approved,
                        custRestType = CustomerRestType.Asian,
                        OwnerID = adminID
                    },
                    new Customer
                    {
                        custName = "TestUser02",
                        custAge = 42,
                        custPhone = "0501234502",
                        UserName = "TestUser02@gmail.com",
                        Status = CustomerStatus.Approved,
                        custRestType = CustomerRestType.Asian,
                        OwnerID = adminID
                    },
                    new Customer
                    {
                        custName = "TestUser03",
                        custAge = 46,
                        custPhone = "0501234503",
                        UserName = "TestUser03@gmail.com",
                        Status = CustomerStatus.Approved,
                        custRestType = CustomerRestType.Desserts,
                        OwnerID = adminID
                    },
                    new Customer
                    {
                        custName = "TestUser04",
                        custAge = 39,
                        custPhone = "0501234504",
                        UserName = "TestUser04@gmail.com",
                        Status = CustomerStatus.Approved,
                        custRestType = CustomerRestType.Desserts,
                        OwnerID = adminID
                    },
                    new Customer
                    {
                        custName = "TestUser05",
                        custAge = 29,
                        custPhone = "0501234505",
                        UserName = "TestUser05@gmail.com",
                        Status = CustomerStatus.Approved,
                        custRestType = CustomerRestType.Desserts,
                        OwnerID = adminID
                    },
                    new Customer
                    {
                        custName = "TestUser06",
                        custAge = 27,
                        custPhone = "0501234506",
                        UserName = "TestUser06@gmail.com",
                        Status = CustomerStatus.Approved,
                        custRestType = CustomerRestType.Europe,
                        OwnerID = adminID
                    },
                    new Customer
                    {
                        custName = "TestUser07",
                        custAge = 36,
                        custPhone = "0501234507",
                        UserName = "TestUser07@gmail.com",
                        Status = CustomerStatus.Approved,
                        custRestType = CustomerRestType.Europe,
                        OwnerID = adminID
                    },
                    new Customer
                    {
                        custName = "TestUser08",
                        custAge = 42,
                        custPhone = "0501234508",
                        UserName = "TestUser08@gmail.com",
                        Status = CustomerStatus.Approved,
                        custRestType = CustomerRestType.Europe,
                        OwnerID = adminID
                    },
                    new Customer
                    {
                        custName = "TestUser09",
                        custAge = 20,
                        custPhone = "0501234509",
                        UserName = "TestUser09@gmail.com",
                        Status = CustomerStatus.Approved,
                        custRestType = CustomerRestType.Mexican,
                        OwnerID = adminID
                    },
                    new Customer
                    {
                        custName = "TestUser10",
                        custAge = 24,
                        custPhone = "0501234510",
                        UserName = "TestUser10@gmail.com",
                        Status = CustomerStatus.Approved,
                        custRestType = CustomerRestType.Fishes,
                        OwnerID = adminID
                    },
                    new Customer
                    {
                        custName = "TestUser11",
                        custAge = 19,
                        custPhone = "0501234511",
                        UserName = "TestUser11@gmail.com",
                        Status = CustomerStatus.Approved,
                        custRestType = CustomerRestType.Fishes,
                        OwnerID = adminID
                    },
                    new Customer
                    {
                        custName = "TestUser12",
                        custAge = 43,
                        custPhone = "0501234512",
                        UserName = "TestUser12@gmail.com",
                        Status = CustomerStatus.Approved,
                        custRestType = CustomerRestType.Fishes,
                        OwnerID = adminID
                    },
                    new Customer
                    {
                        custName = "TestUser13",
                        custAge = 36,
                        custPhone = "0501234513",
                        UserName = "TestUser13@gmail.com",
                        Status = CustomerStatus.Approved,
                        custRestType = CustomerRestType.Homemade,
                        OwnerID = adminID
                    },
                    new Customer
                    {
                        custName = "TestUser14",
                        custAge = 26,
                        custPhone = "0501234514",
                        UserName = "TestUser14@gmail.com",
                        Status = CustomerStatus.Approved,
                        custRestType = CustomerRestType.Homemade,
                        OwnerID = adminID
                    },
                    new Customer
                    {
                        custName = "TestUser15",
                        custAge = 25,
                        custPhone = "0501234515",
                        UserName = "TestUser15@gmail.com",
                        Status = CustomerStatus.Approved,
                        custRestType = CustomerRestType.Homemade,
                        OwnerID = adminID
                    },
                    new Customer
                    {
                        custName = "TestUser16",
                        custAge = 18,
                        custPhone = "0501234516",
                        UserName = "TestUser16@gmail.com",
                        Status = CustomerStatus.Approved,
                        custRestType = CustomerRestType.Israeli,
                        OwnerID = adminID
                    },
                    new Customer
                    {
                        custName = "TestUser17",
                        custAge = 20,
                        custPhone = "0501234517",
                        UserName = "TestUser17@gmail.com",
                        Status = CustomerStatus.Approved,
                        custRestType = CustomerRestType.Israeli,
                        OwnerID = adminID
                    },
                    new Customer
                    {
                        custName = "TestUser18",
                        custAge = 67,
                        custPhone = "0501234518",
                        UserName = "TestUser18@gmail.com",
                        Status = CustomerStatus.Approved,
                        custRestType = CustomerRestType.Italian,
                        OwnerID = adminID
                    },
                    new Customer
                    {
                        custName = "TestUser19",
                        custAge = 59,
                        custPhone = "0501234519",
                        UserName = "TestUser19@gmail.com",
                        Status = CustomerStatus.Approved,
                        custRestType = CustomerRestType.Italian,
                        OwnerID = adminID
                    },
                    new Customer
                    {
                        custName = "TestUser20",
                        custAge = 40,
                        custPhone = "0501234520",
                        UserName = "TestUser20@gmail.com",
                        Status = CustomerStatus.Approved,
                        custRestType = CustomerRestType.Meat,
                        OwnerID = adminID
                    },
                    new Customer
                    {
                        custName = "TestUser21",
                        custAge = 43,
                        custPhone = "0501234521",
                        UserName = "TestUser21@gmail.com",
                        Status = CustomerStatus.Approved,
                        custRestType = CustomerRestType.Meat,
                        OwnerID = adminID
                    },
                    new Customer
                    {
                        custName = "TestUser22",
                        custAge = 44,
                        custPhone = "0501234522",
                        UserName = "TestUser22@gmail.com",
                        Status = CustomerStatus.Approved,
                        custRestType = CustomerRestType.Mediterranean,
                        OwnerID = adminID
                    },
                    new Customer
                    {
                        custName = "TestUser23",
                        custAge = 46,
                        custPhone = "0501234523",
                        UserName = "TestUser23@gmail.com",
                        Status = CustomerStatus.Approved,
                        custRestType = CustomerRestType.Mediterranean,
                        OwnerID = adminID
                    },
                    new Customer
                    {
                        custName = "TestUser24",
                        custAge = 22,
                        custPhone = "0501234524",
                        UserName = "TestUser24@gmail.com",
                        Status = CustomerStatus.Approved,
                        custRestType = CustomerRestType.Mexican,
                        OwnerID = adminID
                    });
                context.SaveChanges();
            }
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
                    restType = RestType.Fishes,
                    restKosher = true,
                    restLat = 32.089348,
                    restLng = 34.797386,
                },

                new Restaurant
                {
                    restName = "Yaffo Tel Aviv",
                    restAddress = "Yigal Alon 98",
                    restCity = "Tel Aviv",
                    restRating = 3,
                    restType =  RestType.Israeli,
                    restKosher = true,
                    restLat = 32.070353,
                    restLng = 34.794209,
                },
            
                new Restaurant
                {
                    restName = "Mashya",
                    restAddress = "5 Mendeli st.",
                    restCity = "Tel Aviv",
                    restRating = 4,
                    restType =  RestType.Mediterranean,
                    restKosher = false,
                    restLat= 32.079671,
                    restLng= 34.768773,
                },
                new Restaurant
                {
                    restName = "MachneYuda",
                    restAddress = "Beit Jacob 10",
                    restCity = "Jerusalem",
                    restRating = 3,
                    restType =  RestType.Israeli,
                    restKosher = false,
                    restLat = 31.785449,
                    restLng = 35.211158,
                },
                new Restaurant
                {
                    restName = "Claro",
                    restAddress = "Ha''arbaa st 23",
                    restCity = "Tel Aviv",
                    restRating = 4,
                    restType =  RestType.Mexican,
                    restKosher = false,
                    restLat=32.070930,
                    restLng=34.787801,
                },
                new Restaurant
                {
                    restName = "Taizu",
                    restAddress = "Menahim Begin 23",
                    restCity = "Tel Aviv",
                    restRating = 5,
                    restType =  RestType.Asian,
                    restKosher = false,
                    restLat=32.063943,
                    restLng=34.780015,
                },
                new Restaurant
                {
                    restName = "Pronto",
                    restAddress = "Hertzel 6",
                    restCity = "Tel Aviv",
                    restRating = 4,
                    restType = RestType.Italian,
                    restKosher = false,
                    restLat=32.063641,
                    restLng=34.769784,
                },
                new Restaurant
                {
                    restName = "Girrafe",
                    restAddress = "Ibn Gabirol 49",
                    restCity = "Tel Aviv",
                    restRating = 3,
                    restType = RestType.Asian,
                    restKosher = false,
                    restLat=32.077274,
                    restLng=34.781233,
                },
                new Restaurant
                {
                    restName = "The Old Lady",
                    restAddress = "Amiad 14",
                    restCity = "Tel Aviv",
                    restRating = 5,
                    restType = RestType.Fishes,
                    restKosher = false,
                    restLat=32.053589,
                    restLng=34.755772,
                },
                new Restaurant
                {
                    restName = "Rustico",
                    restAddress = "Rothschild blvd. 15",
                    restCity = "Tel Aviv",
                    restRating = 4.5,
                    restType = RestType.Italian,
                    restKosher = true,
                    restLat=32.063546,
                    restLng=34.770959,
                },
                new Restaurant
                {
                    restName = "2C",
                    restAddress = "Menachem Begin 132",
                    restCity = "Tel Aviv",
                    restRating = 5,
                    restType = RestType.Mediterranean,
                    restKosher = true,
                    restLat = 32.074774,
                    restLng= 34.791573,
                },
                new Restaurant
                {
                    restName = "Alabama",
                    restAddress = "Pinkas David 47",
                    restCity = "Netanya",
                    restRating = 5,
                    restType = RestType.Israeli,
                    restKosher = false,
                    restLat=32.314429,
                    restLng=34.873948,
                },
                new Restaurant
                {
                    restName = "Limousine",
                    restAddress = "Horesh HaAlonim St 14",
                    restCity = "Ramat Yishai",
                    restRating = 4,
                    restType = RestType.Italian,
                    restKosher = false,
                    restLat= 32.709285,
                    restLng= 35.174259,
                },
                new Restaurant
                {
                    restName = "Whale",
                    restAddress = "Ha-Yam St. 6",
                    restCity = "EirestLat",
                    restRating = 4,
                    restType =  RestType.Fishes,
                    restKosher = false,
                    restLat= 29.548329,
                    restLng= 34.967564,
                },
                new Restaurant
                {
                    restName = "Santa Katarina",
                    restAddress = "Har Sinai 2",
                    restCity = "Tel Aviv",
                    restRating = 4,
                    restType = RestType.Mexican,
                    restKosher = false,
                    restLat= 32.064640,
                    restLng= 34.771773,
                },
                new Restaurant
                {
                    restName = "Ouzeria",
                    restAddress = "Metalon 44",
                    restCity = "Tel Aviv",
                    restRating = 3,
                    restType =  RestType.Homemade,
                    restKosher = false,
                    restLat= 32.059046,
                    restLng= 34.772716,
                },
                new Restaurant
                {
                    restName = "Dallal",
                    restAddress = "Shabazi 10",
                    restCity = "Tel Aviv",
                    restRating = 5,
                    restType =  RestType.Israeli,            
                    restKosher = false,
                    restLat=32.061327,
                    restLng= 34.763459,
                },
                new Restaurant
                {
                    restName = "Suzana",
                    restAddress = "Shabazi 9",
                    restCity = "Tel Aviv",
                    restRating = 4,
                    restType =  RestType.Homemade,
                    restKosher = false,
                    restLat= 32.061646,
                    restLng= 34.763607,
                },
                new Restaurant
                {
                    restName = "Golda",
                    restAddress = "Mikve Israel 18",
                    restCity = "Tel Aviv",
                    restRating = 5,
                    restType =  RestType.Israeli,
                    restKosher = false,
                    restLat= 32.062874,
                    restLng= 34.776297,
                },
                new Restaurant
                {
                    restName = "Aldo",
                    restAddress = "Allenby 1",
                    restCity = "Tel Aviv",
                    restRating = 4,
                    restType = RestType.Desserts,
                    restKosher = false,
                    restLat=32.069460,
                    restLng=34.783678,
                },
                new Restaurant
                {
                    restName = "Taqueria",
                    restAddress = "Levontin 28",
                    restCity = "Tel Aviv",
                    restRating = 5,
                    restType =  RestType.Europe,
                    restKosher = false,
                    restLat=32.063193,
                    restLng=34.776675,
                },
                new Restaurant
                {
                    restName = "Jericho",
                    restAddress = "Pinchas Ben Yair 4",
                    restCity = "Tel Aviv",
                    restRating = 5,
                    restType = RestType.Meat,
                    restKosher = false,
                    restLat=32.090798,
                    restLng=34.781022,
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
                    dishImage = "/images/dishes/Cheese stuffed mushrooms.jpg"
                },
                new Dish
                {
                    dishName = "Fish & Chips",
                    dishCost = 60,
                    dishRating = 3,
                    dishType = "First dish",
                    dishIngredients = "Bass fish, Potatoes, Coriander, Garlic, Onions, Carrot",
                    dishImage = "/images/dishes/Fish & Chips.jpg"
                },
                new Dish
                {
                    dishName = "Carpaccio",
                    dishCost = 60,
                    dishRating = 5,
                    dishType = "First dish",
                    dishIngredients = "Beef tenderloin, Arugala, Vinaigrette sauce, Parmezan",
                    dishImage = "/images/dishes/Carpaccio.jpg"
                },
                new Dish
                {
                    dishName = "Chicken salad",
                    dishCost = 54,
                    dishRating = 4,
                    dishType = "First dish",
                    dishIngredients = "Chicken breast, Tomatoes, Lettuce, Cucumbers, Red peppers, Onions, Vinaigrette sauce",
                    dishImage = "/images/dishes/Chicken salad.jpg"
                },
                new Dish
                {
                    dishName = "Thai Style Sweet Chili",
                    dishCost = 59,
                    dishRating = 3,
                    dishType = "First dish",
                    dishIngredients = "Sweet chili sauce, Chicken wings, Parsley",
                    dishImage = "/images/dishes/Thai Style Sweet Chili.jpg"
                },
                new Dish
                {
                    dishName = "Sushi tempura",
                    dishCost = 56,
                    dishRating = 4,
                    dishType = "First dish",
                    dishIngredients = "Rice, Salmon fish, Avocado, Cream cheese, Tamago, Panko bread crumbs",
                    dishImage = "/images/dishes/Sushi tempura.jpg"
                },
                new Dish
                {
                    dishName = "Greek Salad",
                    dishCost = 49,
                    dishRating = 4,
                    dishType = "First dish",
                    dishIngredients = "Lettuce, Cucumbers, Red peppers, Onions, Tomatoes, Feta cheese, Fried mushrooms",
                    dishImage = "/images/dishes/Greek Salad.jpg"
                },
                new Dish
                {
                    dishName = "Chicken breast",
                    dishCost = 75,
                    dishRating = 5,
                    dishType = "Main dish",
                    dishIngredients = "Chicken breast, Peppers, Salt, Sweet potato fries, BBQ sauce",
                    dishImage = "/images/dishes/Chicken breast.jpg"
                },
                new Dish
                {
                    dishName = "Spaghetti alfredo",
                    dishCost = 69,
                    dishRating = 4,
                    dishType = "Main dish",
                    dishIngredients = "Mushrooms, Sour cream, Onions, Black pepper, Parmesan cheese",
                    dishImage = "/images/dishes/Spaghetti alfredo.jpg"
                },
                new Dish
                {
                    dishName = "Spaghetti bolognese",
                    dishCost = 69,
                    dishRating = 5,
                    dishType = "Main dish",
                    dishIngredients = "Beef, Tomatoes sauce, Onions, Carrots, Coriander, Paprika",
                    dishImage = "/images/dishes/Spaghetti bolognese.jpg"
                },
                new Dish
                {
                    dishName = "chicken curry",
                    dishCost = 75,
                    dishRating = 5,
                    dishType = "Main dish",
                    dishIngredients = "Green curry, Cabbage, Sprouts, Peas, coconut milk",
                    dishImage = "/images/dishes/chicken curry.jpg"
                },
                new Dish
                {
                    dishName = "Pad thai",
                    dishCost = 75,
                    dishRating = 4,
                    dishType = "Main dish",
                    dishIngredients = "Sprouts, Carrot, Mushrooms, Cabbage, Red pepper",
                    dishImage = "/images/dishes/Pad thai.jpg"
                },
                new Dish
                {
                    dishName = "Hamburger",
                    dishCost = 84,
                    dishRating = 5,
                    dishType = "Main dish",
                    dishIngredients = "Beef, Tomatoes, Onions, Lettuce, Pickles, Mayonnaise",
                    dishImage = "/images/dishes/Hamburger.jpg"
                },
                new Dish
                {
                    dishName = "Salmon fillet",
                    dishCost = 110,
                    dishRating = 5,
                    dishType = "Main dish",
                    dishIngredients = "Salmon, Lemon juice, Parsley, English black pepper, Olive oil",
                    dishImage = "/images/dishes/Salmon fillet.jpg"
                },
                new Dish
                {
                    dishName = "Schnitzel with fries",
                    dishCost = 89,
                    dishRating = 5,
                    dishType = "Main dish",
                    dishIngredients = "Chicken schnitzel, Bread crumbs, Potatoes",
                    dishImage = "/images/dishes/Schnitzel with fries.jpg"
                },
                new Dish
                {
                    dishName = "Entrecote",
                    dishCost = 120,
                    dishRating = 5,
                    dishType = "Main dish",
                    dishIngredients = "Beef entrecote",
                    dishImage = "/images/dishes/Entrecote.jpg"
                },
                new Dish
                {
                    dishName = "ChocorestLate souffle",
                    dishCost = 39,
                    dishRating = 5,
                    dishType = "Desert",
                    dishIngredients = "Heavy cream, Dark chocorestLate, Milk, Flour",
                    dishImage = "/images/dishes/ChocorestLate souffle.jpg"
                },
                new Dish
                {
                    dishName = "Creme brulee",
                    dishCost = 39,
                    dishRating = 5,
                    dishType = "Desert",
                    dishIngredients = "Brown sugar, Heavy cream, Eggs, Bananas",
                    dishImage = "/images/dishes/Creme brulee.jpg"
                },
                new Dish
                {
                    dishName = "Cheesecake with cranberries",
                    dishCost = 39,
                    dishRating = 5,
                    dishType = "Desert",
                    dishIngredients = "Cracker crumbs, Cream cheese, Eggs, Vanilla, Cranberries sauce",
                    dishImage = "/images/dishes/Cheesecake with cranberries.jpg"
                },
                new Dish
                {
                    dishName = "Pavlova",
                    dishCost = 39,
                    dishRating = 5,
                    dishType = "Desert",
                    dishIngredients = "Whipped cream , Fresh strawberries, Eggs",
                    dishImage = "/images/dishes/Pavlova.jpg"
                },
                new Dish
                {
                    dishName = "Coca Cola",
                    dishCost = 14,
                    dishRating = 0,
                    dishType = "Beverage",
                    dishIngredients = null,
                    dishImage = "/images/dishes/Coca Cola.jpg"
                },
                new Dish
                {
                    dishName = "Water",
                    dishCost = 10,
                    dishRating = 0,
                    dishType = "Beverage",
                    dishIngredients = null,
                    dishImage = "/images/dishes/Water.jpg"
                },
                new Dish
                {
                    dishName = "Orange juice",
                    dishCost = 17,
                    dishRating = 0,
                    dishType = "Beverage",
                    dishIngredients = null,
                    dishImage = "/images/dishes/Orange juice.jpg"
                },
                new Dish
                {
                    dishName = "Sprite",
                    dishCost = 14,
                    dishRating = 0,
                    dishType = "Beverage",
                    dishIngredients = null,
                    dishImage = "/images/dishes/Sprite.jpg"
                });
            context.SaveChanges();
        }
        #endregion

        #region HashPasswords
        public static string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        } 
        #endregion

        #region UpdateCustomersPasswords
        public static void UpdateCustomersPasswords(ApplicationDbContext context, String password)
        {
            var customers = from m in context.Users
                            where m.UserName != "admin@restit.com" &&
                            m.UserName != "manager@restit.com" &&
                            m.UserName != null
                            select m;

            foreach (var customer in customers)
            {
                customer.PasswordHash = HashPassword(password);
            }

            context.SaveChanges();
        } 
        #endregion
    }
}
