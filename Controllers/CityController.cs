using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practice_03_07.Data;
using Practice_03_07.Models;
using Practice_03_07.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Practice_03_07.Controllers
{
    public class CityController : Controller
    {
        private readonly AppDb _db;

        public CityController(AppDb db)
        {
            _db = db;

        }
        public IActionResult Index()
        {
            IEnumerable<City> objList = _db.Cities.Include(e => e.Country);
            return View(objList);
        }

        [HttpGet]
        public IActionResult Upsert(int? id)
        {
             CityVM cityVM = new CityVM()
            {
                City = new City(),
                CountryList = _db.Countries.Select(e => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Text = e.CountryName,
                    Value = e.Id.ToString()
                })
                

             };
            if (id == null)
            {
                //create
                return View(cityVM);
            }
            else
            {
                cityVM.City = _db.Cities.Find(id);
                if (cityVM.City == null)
                {
                    return NotFound();
                }
                return View(cityVM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Upsert(CityVM cityVM)
        {
            if (ModelState.IsValid)
            {
                if (cityVM.City.Id == 0)
                {
                    //create
                    _db.Cities.Add(cityVM.City);
                }
                else
                {
                    //update
                    var objfromdb = _db.Cities.AsNoTracking().FirstOrDefault(e => e.Id == cityVM.City.Id);

                    if (objfromdb == null)
                    {
                        return NotFound();
                    }
                    _db.Cities.Update(cityVM.City);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            cityVM.CountryList = _db.Countries.Select(u => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Text = u.CountryName,
                Value = u.Id.ToString()
            });
            return View(cityVM);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var obj = _db.Cities.Where(i => i.Id == id).FirstOrDefault();

            _db.Cities.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
