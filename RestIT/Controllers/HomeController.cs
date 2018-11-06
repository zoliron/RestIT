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

        //// Class for GroupBy view
        //public class GroupByRestaurant
        //{
        //    public string RestName { get; set; }
        //    public string RestCity { get; set; }
        //};

        //public void GroupByQuery() {
        //    var result = from restaurant in _context.Restaurant
        //                 group restaurant.restName by restaurant.restCity;

        //    foreach (IGrouping<string, string> item in result)
        //    {
        //        Console.WriteLine(item.Key + ":");
        //        foreach (var r in item)
        //        {
        //            Console.WriteLine(" " + r);
        //        }
        //    }
        //}

        //public IActionResult GroupByQuery() {
        //    var result = from restaurant in _context.Restaurant
        //                  group restaurant.restName by restaurant.restCity into newGroup
        //                  select newGroup;

        //    foreach (IGrouping<string, Restaurant> item in result) {
        //        Console.WriteLine(item.Key + ":");
        //        foreach (Restaurant r in item) {
        //            Console.WriteLine(" " + r.restName);
        //        }
        //    }

        //    //var co = new List<GroupByRestaurant>();
        //    //foreach (var t in result)
        //    //{
        //    //    co.Add(new GroupByRestaurant()
        //    //    {
        //    //        RestName = t.,
        //    //        RestCity = t.restCity
        //    //    });
        //    //}

        //    return View(result);

        //}

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

            // First join between Restaurant and restChef to get the common RestaurantID, restName & restCity
            var result1 = from restaurant in restaurants
                         join restchef in restChefs
                         on restaurant.ID equals restchef.RestaurantID
                         select new
                         {
                             restchef.RestaurantID,
                             restaurant.restName,
                             restaurant.restCity,
                         };

            // Second join between Chef and restChef to get the common ChefID & chefName
            var result2 = from chef in chefs
                          join restchef in restChefs
                          on chef.ID equals restchef.ChefID
                          select new
                          {
                              restchef.ChefID,
                              chef.chefName
                          };

            // Third join between result1 and result2 to get the common ChefID, RestaurantID and display the restName, restCity & chefName
            var result3 = from restaurant in result1
                          join chef in result2
                          on restaurant.RestaurantID equals chef.ChefID
                          select new
                          {
                              restaurant.restName,
                              restaurant.restCity,
                              chef.chefName
                          };


            var co = new List<RestaurantChefs>();
            foreach (var t in result3)
            {
                co.Add(new RestaurantChefs()
                {
                    RestName = t.restName,
                    RestCity = t.restCity,
                    ChefName = t.chefName
                });
            }

            //foreach (var t in result2)
            //{
            //    co.Add(new RestaurantChefs()
            //    {
            //        ChefName = t.chefName
            //    });
            //}

            return View(co);
        }
    }
}
