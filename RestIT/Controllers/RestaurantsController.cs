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

            var restaurants = from m in _context.Restaurant.Include(q => q.Dishes).Include(q => q.restChef)
                              select m;

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
                restaurants = restaurants.Where(x => x.restType.ToString()==(RestaurantType));
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

            var restaurant = await _context.Restaurant.Include(d => d.Dishes).Include(q=>q.restChef)
                .FirstOrDefaultAsync(m => m.ID == id);


            if (restaurant == null)
            {
                TempData["errorMessage"]  = "Restaurant not found. Please try another one."; 
                //return NotFound();
                return RedirectToAction(nameof(Index));
            }
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
        public async Task<IActionResult> Create([Bind("ID,restName,restAddress,restCity,restRating,restType,restKosher,restChef")] Restaurant restaurant,string[] selectedDishes, int[] restChef)
        {
            if (ModelState.IsValid)
            {
                _context.Add(restaurant);
                UpdateDishes(selectedDishes, restaurant, _context);

                var chef = _context.Chef.Single(j => j.ID == restChef[0]);
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

            var restaurant = await _context.Restaurant.Include(q => q.Dishes).Include(q => q.restChef)
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
        public ActionResult Edit(int? id, string[] selectedDishes, int[] restChef, Restaurant rest)
        {
            if (id == null)
            {
                return NotFound();
            }
            var restaurant = _context.Restaurant.Include(q => q.Dishes).Include(q => q.restChef)
                .Where(i => i.ID == id).Single();

            restaurant.restKosher = rest.restKosher;
            restaurant.restAddress = restaurant.restAddress;
            restaurant.restCity = rest.restCity;
            restaurant.restName = rest.restName;
            restaurant.restRating = rest.restRating;
            restaurant.restType = rest.restType;
            restaurant.ID = rest.ID;

            if (ModelState.IsValid)
            {
                try
                {
                    UpdateDishes(selectedDishes, restaurant, _context);
                    var chef = _context.Chef.Single(j => j.ID == restChef[0]); 
                    UpdateChefs(restaurant, chef, _context, false);
                    
                    _context.Update(restaurant);
                    _context.SaveChanges();
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

            var restaurant = await _context.Restaurant.Include(d => d.Dishes).Include(c => c.restChef).Where(j => j.ID == id)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (restaurant == null)
            {
                //return NotFound();
                TempData["errorMessage"] = "Restaurant not found. Please try another one.";
                return RedirectToAction(nameof(Index));
            }

            return View(restaurant);
        }

        // POST: Restaurants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "CustomerAdministrators")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        { 
            var restaurant = await _context.Restaurant.Include(d => d.Dishes)
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

        private void UpdateDishes(string[] selectedDishes, Restaurant restaurant, ApplicationDbContext _context)
        {
            if (restaurant.Dishes == null )
            {
                restaurant.Dishes = new List<Dish>();
            }

            var selectedDishesHS = new HashSet<String>(selectedDishes);
            var restDishesID = new HashSet<int>
                (restaurant.Dishes.Select(c => c.ID));

            foreach (var dish in _context.Dish)
            {
                if (selectedDishesHS.Contains(dish.ID.ToString()))
                {
                    if (!restDishesID.Contains(dish.ID))
                    {
                        restaurant.Dishes.Add(dish);
                    }
                }
                else
                {
                    if (restDishesID.Contains(dish.ID))
                    {
                        restaurant.Dishes.Remove(dish);
                    }
                }
            }
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
    }
}


          
