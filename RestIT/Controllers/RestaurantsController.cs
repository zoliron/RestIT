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

namespace RestIT.Controllers
{
    public class RestaurantsController : Controller
    {
        private readonly ApplicationDbContext _context;
        // GET: Restaurants

        public async Task<IActionResult> Index(string searchString)
        {

            var restaurants = from m in _context.Restaurant
                              select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                restaurants = restaurants.Where(s => s.restName.Contains(searchString));
            }

            return View(_context.Restaurant.Include(q => q.Dishes).ToList());

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

            var restaurant = await _context.Restaurant.Include(d=>d.Dishes)
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
            return View();
        }

        // POST: Restaurants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "CustomerAdministrators")]
        public async Task<IActionResult> Create([Bind("ID,restName,restLocation,restRating,restType,restKosher")] Restaurant restaurant)
        {
            if (ModelState.IsValid)
            {
                _context.Add(restaurant);
                await _context.SaveChangesAsync();
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

            // var restaurant = await _context.Restaurant.Include(q=> q.Dishes).Where(q=>q.ID == id).FirstAsync();
            var restaurant = await _context.Restaurant.Include(q => q.Dishes)
                .Where(i => i.ID == id)
                .FirstAsync(); 
                

            CheckDishes(restaurant, _context);
            if (restaurant == null)
            {
                return NotFound();
            }
            return View(restaurant);
           
        }

        // POST: Restaurants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
            [Authorize(Roles = "CustomerAdministrators")]
        //  public async Task<IActionResult> Edit(int id, string[] selectedDishes, [Bind("ID,restName,restLocation,restRating,restType,restKosher,Dishes")] Restaurant restaurant )
        public ActionResult Edit(int? id, string[] selectedDishes)
        {
            if (id == null)
            {
                return NotFound(); 
            }
            var restaurant = _context.Restaurant.Include(q => q.Dishes)
               .Where(i => i.ID == id)
               .Single();

            if (ModelState.IsValid)
            {
                try
                {
                    UpdateDishes(selectedDishes, restaurant,_context);
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
            //var restaurant = await _context.Restaurant.FindAsync(id);

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

            if ( restaurant.Dishes == null)
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

    }

}


          
