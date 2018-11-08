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

        // Class for GroupBy view
        public class GroupByRestaurantCity
        {
            public string RestName { get; set; }
            public string RestCity { get; set; }
        };

        public IActionResult GroupByRestaurantCityQuery()
        {
            var result = from restaurant in _context.Restaurant
                         group restaurant by restaurant.restCity into newGroup
                         select newGroup;

            var co = new List<GroupByRestaurantCity>();

            foreach (IGrouping<string, Restaurant> item in result)
            {
                foreach (Restaurant r in item)
                {
                    co.Add(new GroupByRestaurantCity()
                    {
                        RestName = r.restName,
                        RestCity = r.restCity
                    });
                }
            }

            //foreach (var r in result)
            //{
            //    co.Add(new GroupByRestaurantCity()
            //    {
            //        RestName = r.Key,
            //    });
            //}

            return View(co);

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

            // JOIN between Comments and Posts
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


            var co = new List<RestaurantChefs>();
            foreach (var t in result2)
            {
                co.Add(new RestaurantChefs()
                {
                    RestName = t.restName,
                    RestCity = t.restCity,
                    ChefName = t.chefName
                });
            }

            return View(co);
        }
    }
}
