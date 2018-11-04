using System;
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

        // Class to JOIN between Restaurant table and Customer table
        public class RestaurantDishes
        {
            public string RestName { get; set; }
            public string RestCity { get; set; }
            public RestType RestType { get; set; }
            public string DishName { get; set; }
            public string DishType { get; set; }
        };

        public IActionResult JoinQueryRestaurantsDishes()
        {

            // JOIN between Comments and Posts
            var restaurants = _context.Restaurant.ToList();
            var dishes = _context.Dish.ToList();

            var result = from restaurant in restaurants
                         join dish in dishes
                         on restaurant.DishID equals dish.ID
                         select new
                         {
                             restaurant.restName,
                             restaurant.restCity,
                             restaurant.restType,
                             dish.dishName,
                             dish.dishType
                         };

            var co = new List<RestaurantDishes>();
            foreach (var t in result)
            {
                co.Add(new RestaurantDishes()
                {
                    RestName = t.restName,
                    RestCity = t.restCity,
                    RestType = t.restType,
                    DishName = t.dishName,
                    DishType = t.dishType
                });
            }

            return View(co);
        }
    }
}
