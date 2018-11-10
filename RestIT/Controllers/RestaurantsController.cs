using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestIT.Data;
using RestIT.Models;
using RestIT.Models.ViewModels;
using Facebook;
using System.Net;
using System.IO;

namespace RestIT.Controllers
{
    public class RestaurantsController : Controller
    {

        private readonly ApplicationDbContext _context;
        private String facebook_token = "EAAEvkEWov44BAAEskSIXey4Gy3dpoKOAEZAoZAXaZA0n0tpgmmGvdFLauSOxsZCRhJX03G9ZBVPBDeHf11ZAZAivqeU0wB4V9jZCLWtgiChdqbjlUylJxWhoX5HTcBXQ1mrs9NAZAsFZBLc994w1rSABjpBoc9e29NLR134tsFFbryyQZDZD";

        // GET: Restaurants
        public async Task<IActionResult> Index(string RestaurantType, string RestaurantCity, string RestaurantChef, double RestaurantRating, string searchString)
        {
            if (TempData["errorMessage"] != null)
            {
                ViewBag.error = TempData["errorMessage"].ToString();
            }
            // Use LINQ to get list of genres.
            IQueryable<string> typeQuery = from m in _context.Restaurant
                                           select m.restType.ToString();

            IQueryable<string> cityQuery = from m in _context.Restaurant
                                           orderby m.restCity
                                           select m.restCity;

            var restaurants = from m in _context.Restaurant
                              .Include(q => q.Dishes)
                              .Include(q => q.restChef)
                              .Include(q => q.RestaurantDishes)
                              select m;

            //var restaurants = from rests in _context.Restaurant.Include(q => q.Dishes).Include(q => q.restChef)
            //                  join chefs in _context.RestaurantChef on rests.restChef.Last().ChefID equals chefs.ChefID
            //                  select new { xRest = rests, xChef = chefs};

            var chefs = from c in _context.RestaurantChef.Include(c => c.Chef)
                        select c;

            if (!String.IsNullOrEmpty(searchString))
            {
                restaurants = restaurants.Where(s => s.restName.Contains(searchString));
            }

            if (!String.IsNullOrEmpty(RestaurantCity))
            {
                restaurants = restaurants.Where(x => x.restCity == RestaurantCity);
            }

            if (!String.IsNullOrEmpty(RestaurantType))
            {
                restaurants = restaurants.Where(x => x.restType.ToString() == (RestaurantType));
            }

            var restaurantSearchVM = new RestaurantSearchViewModel();
            restaurantSearchVM.Types = new SelectList(await typeQuery.Distinct().ToListAsync());
            restaurantSearchVM.Citys = new SelectList(await cityQuery.Distinct().ToListAsync());
            restaurantSearchVM.Restaurants = await restaurants.ToListAsync();
            restaurantSearchVM.SearchString = searchString;

            return View(restaurantSearchVM);
        }

        public RestaurantsController(ApplicationDbContext context)
        {
            _context = context;
        }



        // GET: Restaurants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurant = await _context.Restaurant
                .Include(d => d.Dishes)
                .Include(q=>q.restChef)
                .Include(q => q.RestaurantDishes)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (restaurant == null)
            {
                TempData["errorMessage"] = "Restaurant not found. Please try another one.";
                //return NotFound();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Temp = GetTemperature(restaurant.restLng, restaurant.restLat);
            ViewBag.axes = "lat:" + restaurant.restLat + ", lng:" + restaurant.restLng;

            CheckChefs(restaurant, _context);

            return View(restaurant);
        }

        // GET: Restaurants/Create
        [Authorize(Roles = "CustomerAdministrators")]
        public IActionResult Create()
        {
            Restaurant restaurant = new Restaurant();
            CheckDishes(restaurant, _context);
            CheckChefs(restaurant, _context);

            return View();
        }

        // POST: Restaurants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "CustomerAdministrators")]
        public async Task<IActionResult> Create([Bind("ID,restName,restAddress,restCity,restRating,restLat,restLng,restType,restKosher,restChef")] Restaurant restaurant, string[] selectedDishes, int[] restChef)
        {
            if (ModelState.IsValid)
            {
                var chef = _context.Chef.Single(j => j.ID == restChef[0]);
                _context.Add(restaurant);
                UpdateDishes(selectedDishes, restaurant, _context);
                UpdateChefs(restaurant, chef, _context, true);

                await _context.SaveChangesAsync();

                string fb_message = "Hi, New Restaurant available " + restaurant.restName + " in " + restaurant.restCity + ". Check it out!";

                //Publish post to facebook with restaurant name
                PublishFacebookPost(fb_message);

                return RedirectToAction(nameof(Index));
            }

            return View(restaurant);
        }

        // GET: Restaurants/Edit/5
        [Authorize(Roles = "CustomerAdministrators")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurant = await _context.Restaurant
                .Include(q => q.Dishes)
                .Include(q => q.restChef)
                .Include(q => q.RestaurantDishes)
                .Where(i => i.ID == id).FirstAsync();
            
            if (restaurant == null)
            {
                TempData["errorMessage"] = "Restaurant not found. Please try another one.";
                return RedirectToAction(nameof(Index));
                //return NotFound();
            }
            else
            {
                CheckDishes(restaurant, _context);
                CheckChefs(restaurant, _context);
                return View(restaurant);
            }
        }

        // POST: Restaurants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "CustomerAdministrators")]
        public async Task<IActionResult> Edit(int? id, string[] selectedDishes, int[] restChef, Restaurant rest, Dish dish)
        {
            if (id == null)
            {
                return NotFound();
            }
            var restaurant = _context.Restaurant.Include(q => q.Dishes).Include(q => q.restChef)
                .Where(i => i.ID == id).FirstOrDefault();

            if (restaurant == null)
            {
                TempData["errorMessage"] = "Restaurant not found. Please try another one.";
                return RedirectToAction(nameof(Index));
                //return NotFound();
            }

            restaurant.restKosher = rest.restKosher;
            restaurant.restAddress = restaurant.restAddress;
            restaurant.restCity = rest.restCity;
            restaurant.restLat = rest.restLat;
            restaurant.restLng = rest.restLng;
            restaurant.restName = rest.restName;
            restaurant.restRating = rest.restRating;
            restaurant.restType = rest.restType;
            restaurant.ID = rest.ID;

            if (ModelState.IsValid)
            {
                try
                {
                    var chef = _context.Chef.Single(j => j.ID == restChef[0]);
                    UpdateDishes(selectedDishes, restaurant, _context);
                    UpdateChefs(restaurant, chef, _context, false);

                    _context.Update(restaurant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RestaurantExists(restaurant.ID))
                    {
                        //return NotFound();
                        TempData["errorMessage"] = "Restaurant not found. Please try another one.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(restaurant);
        }

        // GET: Restaurants/Delete/5
        [Authorize(Roles = "CustomerAdministrators")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurant = await _context.Restaurant
                .Include(d => d.Dishes)
                .Include(c => c.restChef)
                .Include(q => q.RestaurantDishes)
                .Where(j => j.ID == id)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (restaurant == null)
            {
                //return NotFound();
                TempData["errorMessage"] = "Restaurant not found. Please try another one.";
                return RedirectToAction(nameof(Index));
            }

            CheckChefs(restaurant, _context);

            return View(restaurant);
        }

        // POST: Restaurants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "CustomerAdministrators")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        { 
            var restaurant = await _context.Restaurant
                .Include(d => d.Dishes)
                .Include(d => d.restChef)
                .Include(q => q.RestaurantDishes)
                .FirstOrDefaultAsync(m => m.ID == id);
            _context.Restaurant.Remove(restaurant);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RestaurantExists(int id)
        {
            return _context.Restaurant.Any(e => e.ID == id);
        }

        private void CheckDishes(Restaurant restaurant, ApplicationDbContext _context)
        {
            if (restaurant.Dishes == null)
            {
                restaurant.Dishes = new List<Dish>();
            }

            var allDishes = _context.Dish;
            var viewModel = new List<Dish>();
            foreach (var dish in allDishes)
            {
                viewModel.Add(new Dish
                {
                    ID = dish.ID,
                    dishName = dish.dishName,
                    assigned = restaurant.Dishes.Contains(dish),
                });


            }
            ViewBag.Dishes = viewModel;
        }

        private void CheckChefs(Restaurant restaurant, ApplicationDbContext context)
        {
            if (restaurant.restChef == null || restaurant.restChef.Count() == 0)
            {
                restaurant.restChef = new List<RestaurantChef>();
                ViewBag.selectedChefName = "None";
            }
            else
            {
                var temp = context.Chef
                   .Include(p => p.Restuarants)
                   .Single(p => p.ID == restaurant.restChef.Last().ChefID);

                ViewBag.selectedChefName = temp.chefName;
            }

            var allChefs = _context.Chef;
            var viewModel = new List<Chef>();
            foreach (var chef in allChefs)
            {
                viewModel.Add(new Chef
                {
                    ID = chef.ID,
                    chefName = chef.chefName,
                });
            }
            ViewBag.restChef = viewModel;
        }

        private void UpdateDishes(string[] selectedDishes, Restaurant Restaurant, ApplicationDbContext _context)
        {
            if (Restaurant.RestaurantDishes == null) {
                Restaurant.RestaurantDishes = new List<RestaurantDish>();
            }
            var selectedDishesHS = new HashSet<String>(selectedDishes);
            var restDishesID = new HashSet<int>(Restaurant.RestaurantDishes.Select(c => c.DishID));

            var list = from m in _context.RestaurantDish
                       select m;

            List<RestaurantDish> RestauntDishList = list.ToList();
            

            foreach (var dish in _context.Dish)
            {
                if (selectedDishesHS.Contains(dish.ID.ToString()))
                {
                    var match = 0;

                    // First initialization
                    if (RestauntDishList.Count == 0) {
                        _context.RestaurantDish.Add(new RestaurantDish
                        {
                            Restaurant = Restaurant,
                            Dish = dish,
                            dishName = dish.dishName
                        });
                    }

                    // Checking if the RestaurantDish already exists.
                    foreach (var item in list) {
                        if (item.DishID == dish.ID && item.RestaurantID == Restaurant.ID)
                        {
                            match = 1;
                            break;
                        }
                    }

                    // Adds RestaurantDish after the first was created
                    if (match == 0 && RestauntDishList.Count != 0)
                    {
                        _context.RestaurantDish.Add(new RestaurantDish
                        {
                            Restaurant = Restaurant,
                            Dish = dish,
                            dishName = dish.dishName
                        });
                    }
                }
                else
                {
                    // Clearing the dishes if you didnt tick the dishes you want
                    foreach (var item in list) {
                        if (item.DishID == dish.ID && item.RestaurantID == Restaurant.ID) {
                            RestaurantDish restDishOld = _context.RestaurantDish.FirstOrDefault(i => i.DishID == dish.ID && i.RestaurantID == Restaurant.ID);
                            if (restDishOld != null)
                            {
                                _context.RestaurantDish.Remove(restDishOld);
                            }
                        }
                    }
                }
            }
            //_context.SaveChangesAsync();
        }

        private void UpdateChefs(Restaurant restaurant, Chef chef, ApplicationDbContext _context, bool Create)
        {
            if (chef == null)
            {
                return;
            }

            if (!Create)
            {
                RestaurantChef restChefOld = _context.RestaurantChef.Single(i => i.RestaurantID == restaurant.ID);
                _context.RestaurantChef.Remove(restChefOld);
                // _context.SaveChangesAsync();

            }
            restaurant.restChef = new List<RestaurantChef>();

            restaurant.restChef.Add(new RestaurantChef {
               //ID = restaurant.restChef.Count + 1,
               Restaurent = restaurant,
               Chef = chef
           });
            //await _context.SaveChangesAsync();
        }

        public Boolean PublishFacebookPost(String facebookMessage)
        {
            try
            {
                var fb = new FacebookClient(facebook_token);

                dynamic result = fb.Post("2477297058954584/feed", new
                {
                    message = facebookMessage
                });
                var newPostId = result.id;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IActionResult> GetAllLocation()
        {
            return Json(await _context.Restaurant.ToListAsync());
        }

        public string GetTemperature(double longtitude, double latitude)
        {
            string html = string.Empty;
            string url = @"http://api.openweathermap.org/data/2.5/weather?lat=" + latitude + "&lon=" + longtitude + "&units=metric&mode=html&APPID=de89ef2331afa98fbd84af7d6be8e8b8"; 

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
                stream.Dispose();
            }

            return html;
        }
    }
}



