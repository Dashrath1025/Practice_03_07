using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practice_03_07.Data;
using Practice_03_07.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Practice_03_07.Controllers
{
    public class TaxController : Controller
    {
        private readonly AppDb _db;
        public TaxController(AppDb db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Tax> objList = _db.Taxes;
            return View(objList);
        }

        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            Tax tax = new Tax();
            if (id == null)
            {
                //create 
                return View(tax);
            }
            else
            {
                tax  = _db.Taxes.Find(id);
                if(tax== null)
                {
                    return NotFound();

                }
                return View(tax);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Upsert(Tax tax)
        {
            if (ModelState.IsValid)
            {
                if(tax.Id== 0)
                {
                    //create
                    _db.Taxes.Add(tax);
                }
                else
                {
                    var objfromd = _db.Taxes.AsNoTracking().FirstOrDefault(e => e.Id == tax.Id);
                    if (objfromd == null)
                    {
                        return NotFound();
                    }
                    _db.Taxes.Update(tax);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");

            }
            return View(tax);
        }


    }
}
