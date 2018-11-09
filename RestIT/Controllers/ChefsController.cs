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

namespace RestIT.Controllers
{
    public class ChefsController : Controller
    {
        private readonly ApplicationDbContext _context;
        
        public async Task<IActionResult> Index(string searchString)
        {
            if (TempData["errorMessage"] != null)
            {
                ViewBag.error = TempData["errorMessage"].ToString();
            }

            var chefs = from m in _context.Chef
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                chefs = chefs.Where(s => s.chefName.Contains(searchString));
            }

            return View(await chefs.ToListAsync());
        }
        

        public ChefsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /*
        // GET: Chefs
        public async Task<IActionResult> Index()
        {

            return View(await _context.Chef.ToListAsync());

        }
        */
        // GET: Chefs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chef = await _context.Chef
                .FirstOrDefaultAsync(m => m.ID == id);
            if (chef == null)
            {
                //return NotFound();
                TempData["errorMessage"] = "Chef not found. Please try another one.";
                return RedirectToAction(nameof(Index));
            }

            return View(chef);
        }

        // GET: Chefs/Create
        [Authorize(Roles = "CustomerAdministrators")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Chefs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "CustomerAdministrators")]
        public async Task<IActionResult> Create([Bind("ID,chefName,Restuarants")] Chef chef)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chef);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(chef);
        }

        // GET: Chefs/Edit/5
        [Authorize(Roles = "CustomerAdministrators")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chef = await _context.Chef.FindAsync(id);
            if (chef == null)
            {
                TempData["errorMessage"] = "Chef not found. Please try another one.";
                return RedirectToAction(nameof(Index));
                //return NotFound();
            }
            return View(chef);
        }

        // POST: Chefs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "CustomerAdministrators")]
        public async Task<IActionResult> Edit(int id, [Bind("ID,chefName,Restuarants")] Chef chef)
        {
            if (id != chef.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chef);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChefExists(chef.ID))
                    {
                        TempData["errorMessage"] = "Chef not found. Please try another one.";
                        return RedirectToAction(nameof(Index));
                        //return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(chef);
        }

        // GET: Chefs/Delete/5
        [Authorize(Roles = "CustomerAdministrators")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chef = await _context.Chef
                .FirstOrDefaultAsync(m => m.ID == id);
            if (chef == null)
            {
                TempData["errorMessage"] = "Chef not found. Please try another one.";
                return RedirectToAction(nameof(Index));
                //return NotFound();
            }

            return View(chef);
        }

        // POST: Chefs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "CustomerAdministrators")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chef = await _context.Chef.FindAsync(id);
            _context.Chef.Remove(chef);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChefExists(int id)
        {
            return _context.Chef.Any(e => e.ID == id);
        }
    }
}
