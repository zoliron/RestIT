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
    public class DishesController : Controller
    {
        private readonly ApplicationDbContext _context;
        public async Task<IActionResult> Index(string searchString, string DishType)
        {
            if (TempData["errorMessage"] != null)
            {
                ViewBag.error = TempData["errorMessage"].ToString();
            }

            var dishes = from m in _context.Dish
                              select m;

            IQueryable<string> typeQuery = from m in _context.Dish
                                           orderby m.dishType
                                           select m.dishType;

            if (!String.IsNullOrEmpty(searchString))
            {
                dishes = dishes.Where(s => s.dishName.Contains(searchString));
            }

            if (!String.IsNullOrEmpty(DishType))
            {
                dishes = dishes.Where(x => x.dishType == DishType);
            }
            
            var dishSearchVM = new DishSearchViewModel();
            dishSearchVM.Types = new SelectList(await typeQuery.Distinct().ToListAsync());
            dishSearchVM.Dishes = await dishes.ToListAsync();
            dishSearchVM.SearchString = searchString;

            return View(dishSearchVM);
        }

        public DishesController(ApplicationDbContext context)
        {
            _context = context;
        }
        /*
        // GET: Dishes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Dish.ToListAsync());
        }
        */
        // GET: Dishes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dish
                .FirstOrDefaultAsync(m => m.ID == id);


            if (dish == null)
            {
                TempData["errorMessage"] = "Dish not found. Please try another one.";
                return RedirectToAction(nameof(Index));
                //return NotFound();
            }
            GenerateImageSrc(dish, 304, 228);

            return View(dish);
        }

        // GET: Dishes/Create
        [Authorize(Roles = "CustomerAdministrators")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Dishes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
       [Authorize(Roles = "CustomerAdministrators")]
        public async Task<IActionResult> Create([Bind("ID,dishName,dishCost,dishRating,dishType,dishIngredients,dishImage")] Dish dish)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dish);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dish);
        }

        // GET: Dishes/Edit/5
        [Authorize(Roles = "CustomerAdministrators")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dish.FindAsync(id);
            if (dish == null)
            {
                TempData["errorMessage"] = "Dish not found. Please try another one.";
                return RedirectToAction(nameof(Index));
              //  return NotFound();
            }
            return View(dish);
        }

        // POST: Dishes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "CustomerAdministrators")]
        public async Task<IActionResult> Edit(int id, [Bind("ID,dishName,dishCost,dishRating,dishType,dishIngredients,dishImage")] Dish dish)
        {
            if (id != dish.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dish);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DishExists(dish.ID))
                    {
                        TempData["errorMessage"] = "Dish not found. Please try another one.";
                        return RedirectToAction(nameof(Index));
                        // return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(dish);
        }

        // GET: Dishes/Delete/5
        [Authorize(Roles = "CustomerAdministrators")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dish
                .FirstOrDefaultAsync(m => m.ID == id);
            if (dish == null)
            {
                TempData["errorMessage"] = "Dish not found. Please try another one.";
                return RedirectToAction(nameof(Index));
                //return NotFound();
            }

            return View(dish);
        }

        // POST: Dishes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "CustomerAdministrators")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dish = await _context.Dish.FindAsync(id);
            _context.Dish.Remove(dish);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DishExists(int id)
        {
            return _context.Dish.Any(e => e.ID == id);
        }

        private void GenerateImageSrc(Dish dish, int width, int height)
        {
            if (dish == null)
            {
                return;
            }

            // && System.IO.File.Exists(dish.dishImage))

            if (dish.dishImage != null)
            {
                string viewModel = "<img src=\"" + dish.dishImage + "\" alt=\"" + dish.dishName +
                "\" width=\"" + width + "\" height=\"" + height+ "\">";
                ViewBag.dishImage = viewModel;
            }
        }
    }
}
