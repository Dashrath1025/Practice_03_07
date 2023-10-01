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
    public class SubCategoryController : Controller
    {
        public AppDb _db;

        public SubCategoryController(AppDb db)
        {
            _db = db;

        }
        public IActionResult Index()
        {
            IEnumerable<SubCategory> objList = _db.subCategories.Include(e => e.Category);
            return View(objList);
        }

        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            if (ModelState.IsValid)
            {
                //   ViewBag.Select = _db.Categories;
                SubCategoryVM subCategoryVM = new SubCategoryVM()
                {
                    SubCategory = new SubCategory(),
                    CategoryList = _db.Categories.Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    })
                };

                if (id == null)
                {
                    //create
                    return View(subCategoryVM);
                }
                else
                {
                    //update
                    subCategoryVM.SubCategory = _db.subCategories.Find(id);
                    if (subCategoryVM.SubCategory == null)
                    {
                        return NotFound();
                    }
                    return View(subCategoryVM);
                }
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Upsert(SubCategoryVM subCategoryVM)
        {
            if (ModelState.IsValid)
            {
                if (subCategoryVM.SubCategory.Id == 0)
                {
                    //create
                    _db.subCategories.Add(subCategoryVM.SubCategory);
                }
                else
                {
                    var objFromDb = _db.subCategories.AsNoTracking().FirstOrDefault(e => e.Id == subCategoryVM.SubCategory.Id);
                    if(objFromDb == null)
                    {
                        return NotFound();

                    }
                    _db.subCategories.Update(subCategoryVM.SubCategory);
                }

                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            subCategoryVM.CategoryList = _db.Categories.Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            return View(subCategoryVM);
        }

        public IActionResult Delete(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }

            var obj = _db.subCategories.Where(e => e.Id == id).FirstOrDefault();

            if(obj == null)
            {
                return NotFound();
            }

            _db.subCategories.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}
