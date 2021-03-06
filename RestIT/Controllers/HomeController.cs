﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestIT.Data;
using RestIT.Models;

namespace RestIT.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Facebook()
        {
            return View();
        }

        // Class for GroupBy view
        public class GroupByRestaurant
        {
            public RestType RestType { get; set; }
            public string RestCity { get; set; }
            public int RestCount { get; set; }
        };

        // GroupBy restaurant cities
        public IActionResult GroupByRestaurantCityQuery()
        {
            var result = from restaurant in _context.Restaurant
                         select restaurant;

            var restaurantCities = new List<GroupByRestaurant>();

            foreach (var r in result.GroupBy(info => info.restCity)
                        .Select(group => new
                        {
                            RestCity = group.Key,
                            RestCount = group.Count(),
                        })
                        .OrderBy(x => x.RestCity))
            {
                restaurantCities.Add(new GroupByRestaurant()
                {
                    RestCity = r.RestCity,
                    RestCount = r.RestCount
                });

                ViewBag.data += r.RestCity + "," + r.RestCount + ",";
            }

            return View(restaurantCities);

        }

        // GroupBy restaurant types
        public IActionResult GroupByRestaurantTypeQuery()
        {
            var result = from restaurant in _context.Restaurant
                         select restaurant;

            var restaurantTypes = new List<GroupByRestaurant>();

            foreach (var r in result.GroupBy(info => info.restType)
                        .Select(group => new
                        {
                            RestType = group.Key,
                            RestCount = group.Count(),
                        })
                        .OrderBy(x => x.RestType))
            {
                restaurantTypes.Add(new GroupByRestaurant()
                {
                    RestType = r.RestType,
                    RestCount = r.RestCount
                });

                ViewBag.data += r.RestType + "," + r.RestCount + ",";

            }


            return View(restaurantTypes);

        }

        // Class to JOIN between Restaurant table and Chefs table
        public class RestaurantChefs
        {
            public string RestName { get; set; }
            public string RestCity { get; set; }
            public string ChefName { get; set; }

        };

        public IActionResult JoinQueryRestaurantsChefs()
        {
            var restaurants = _context.Restaurant.ToList();
            var chefs = _context.Chef.ToList();
            var restChefs = _context.RestaurantChef.ToList();

            // First join between Restaurant and restChef to get ChefID, restName & restCity using common RestaurantID
            var result1 = from restaurant in restaurants
                          join restchef in restChefs
                          on restaurant.ID equals restchef.RestaurantID
                          select new
                          {
                              restchef.ChefID,
                              restaurant.restName,
                              restaurant.restCity,
                          };

            // First join between Chef and result1 to get restName, restCity & chefName using common ChefID
            var result2 = from chef in chefs
                          join restaurant in result1
                          on chef.ID equals restaurant.ChefID
                          select new
                          {
                              restaurant.restName,
                              restaurant.restCity,
                              chef.chefName
                          };


            var restaurantChefs = new List<RestaurantChefs>();
            foreach (var t in result2)
            {
                restaurantChefs.Add(new RestaurantChefs()
                {
                    RestName = t.restName,
                    RestCity = t.restCity,
                    ChefName = t.chefName
                });
            }

            return View(restaurantChefs);
        }

        // Class to JOIN between Restaurant table and Dishes table
        public class RestaurantDishes
        {
            public string RestName { get; set; }
            public string RestCity { get; set; }
            public string DishName { get; set; }
            public int DishCost { get; set; }
        };

        public IActionResult JoinQueryRestaurantsDishes()
        {
            var restaurants = _context.Restaurant.ToList();
            var dishes = _context.Dish.ToList();
            var restDishes = _context.RestaurantDish.ToList();

            // First join between Restaurant and RestaurantDishes to get restName & dishName using common RestaurantID
            var result1 = from restaurant in restaurants
                         join restdish in restDishes
                         on restaurant.ID equals restdish.RestaurantID
                         select new
                         {
                             restdish.DishID,
                             restaurant.restName,
                             restaurant.restCity,
                         };

            // First join between Dish and result1 to get restName, restCity, dishName & dishCost using common DishID
            var result2 = from dish in dishes
                          join restaurant in result1
                          on dish.ID equals restaurant.DishID
                          select new
                          {
                              restaurant.restName,
                              restaurant.restCity,
                              dish.dishName,
                              dish.dishCost
                          };


            var restaurantDishes = new List<RestaurantDishes>();
            foreach (var t in result2)
            {
                restaurantDishes.Add(new RestaurantDishes()
                {
                    RestName = t.restName,
                    RestCity = t.restCity,
                    DishName = t.dishName,
                    DishCost = t.dishCost
                });
            }

            return View(restaurantDishes);
        }
    }
}