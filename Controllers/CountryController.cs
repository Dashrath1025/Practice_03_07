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
    public class CountryController : Controller
    {

        private readonly AppDb _db;

        public CountryController(AppDb db)
        {
            _db = db;

        }
        public IActionResult Index()
        {
            IEnumerable<Country> objList = _db.Countries;
            return View(objList);
        }

        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            Country country = new Country();

            if (id == null)
            {
                return View(country);
            }
            else
            {
                country = _db.Countries.Find(id);
                if (country == null)
                {
                    return NotFound();
                }
                return View(country);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Upsert(Country country)
        {
            if (ModelState.IsValid)
            {
                if (country.Id == 0)
                {
                    //create
                    _db.Countries.Add(country);
                }
                else
                {
                    var objfromdb = _db.Countries.AsNoTracking().FirstOrDefault(e => e.Id == country.Id);

                    if (objfromdb == null)
                    {
                        return NotFound();
                    }
                    _db.Countries.Update(country);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(country);
        }

        public IActionResult Delete(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }

            var obj = _db.Countries.Where(i => i.Id == id).FirstOrDefault();

            _db.Countries.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
