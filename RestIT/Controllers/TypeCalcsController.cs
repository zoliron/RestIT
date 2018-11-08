using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestIT.Data;
using RestIT.Models;

namespace RestIT.Controllers
{
    public class TypeCalcsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TypeCalcsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TypeCalcs
        public async Task<IActionResult> Index()
        {
            return View(await _context.TypeCalc.ToListAsync());
        }

      
        // GET: TypeCalcs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TypeCalcs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,sex,age,city,Type")] TypeCalc typeCalc)
        {
            if (ModelState.IsValid)
            {
                ViewBag.result1 = "Predicted favorite restaurant type : "; 
                ML model = new ML();
                ViewBag.result2 = model.Run(new TrainData
                {
                    City = (float)typeCalc.city,
                    Sex = (float)typeCalc.sex,
                    Age = (float)typeCalc.age,


                });
                ViewBag.result3 = "You'll probably enjoy being there on a : ";

                Hang hang = new Hang(); 
                ViewBag.result4 = hang.Run(new HangType
                {
                    City = (float)typeCalc.city,
                    Sex = (float)typeCalc.sex,
                    Age = (float)typeCalc.age,


                });



                return View();
            }
            return View(typeCalc);
        }
        

        private bool TypeCalcExists(int id)
        {
            return _context.TypeCalc.Any(e => e.ID == id);
        }
    }
}
