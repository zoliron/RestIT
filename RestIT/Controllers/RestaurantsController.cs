﻿using System;
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
using RestIT.ViewModels;
using Facebook;

namespace RestIT.Controllers
{
    public class RestaurantsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private String facebook_token = "";
        
        // GET: Restaurants
        public async Task<IActionResult> Index(string RestaurantType, string RestaurantCity, string RestaurantChef, double RestaurantRating, string searchString)
        {
            // Use LINQ to get list of genres.
            IQueryable<string> typeQuery = from m in _context.Restaurant
                                           orderby m.restType
                                            select m.restType;

            IQueryable<string> cityQuery = from m in _context.Restaurant
                                               orderby m.restCity
                                               select m.restCity;

            var restaurants = from m in _context.Restaurant.Include(q => q.Dishes)
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
                restaurants = restaurants.Where(x => x.restType == RestaurantType);
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

            var restaurant = await _context.Restaurant.Include(d => d.Dishes)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (restaurant == null)
            {
                return NotFound();
            }

            return View(restaurant);
        }

        // GET: Restaurants/Create
        [Authorize(Roles = "CustomerAdministrators")]
        public IActionResult Create()
        {
            CheckDishes(new Restaurant(), _context);
            return View();
        }

        // POST: Restaurants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "CustomerAdministrators")]
        public async Task<IActionResult> Create([Bind("ID,restName,restLocation,restRating,restType,restKosher,restChef")] Restaurant restaurant,string[] selectedDishes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(restaurant);
                UpdateDishes(selectedDishes, restaurant, _context);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

                string fb_message = "Hi, New Restaurant Availabe " + restaurant.restName + " Check it out!";

                //Publish post to facebook with restaurant name
                PublishFacebookPost(fb_message);
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

            // var restaurant = await _context.Restaurant.Include(q=> q.Dishes).Where(q=>q.ID == id).FirstAsync();
            //var restaurant = await _context.Restaurant.Include(q => q.Dishes)
            //  .Where(i => i.ID == id)
            //.FirstAsync();

            var restaurant = await _context.Restaurant.FindAsync(id);

            CheckDishes(restaurant, _context);
            if (restaurant == null)
            {
                return NotFound();
            }
            else
            {
                var getChefs = from chef in _context.Chef select chef;
                var chefs = await getChefs.ToListAsync();

                RestaurantViewModel restaurantViewModel = new RestaurantViewModel(restaurant, chefs);
                return View(restaurantViewModel);
            }
        }

        // POST: Restaurants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "CustomerAdministrators")]
        //  public async Task<IActionResult> Edit(int id, string[] selectedDishes, [Bind("ID,restName,restLocation,restRating,restType,restKosher,Dishes")] Restaurant restaurant )
        public ActionResult Edit(int? id, string[] selectedDishes, Restaurant rest)
        {
            if (id == null)
            {
                return NotFound();
            }
            var restaurant = _context.Restaurant.Include(q => q.Dishes)
               .Where(i => i.ID == id)
               .Single();
            //Restaurant restaurant = new Restaurant();
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
                    _context.Update(restaurant);
                    // await _context.SaveChangesAsync();
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RestaurantExists(restaurant.ID))
                    {
                        return NotFound();
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

            var restaurant = await _context.Restaurant.Include(d => d.Dishes)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (restaurant == null)
            {
                return NotFound();
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

        private void UpdateDishes(string[] selectedDishes, Restaurant restaurant, ApplicationDbContext _context)
        {
            if (selectedDishes == null )
            {
                restaurant.Dishes = new List<Dish>();
                return;
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
        public Boolean PublishFacebookPost(String facebookMessage)
        {
            try
            {
                var fb = new FacebookClient(facebook_token);

                dynamic result = fb.Post("me/feed", new
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


    }

}


          
