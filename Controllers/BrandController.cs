using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practice_03_07.Data;
using Practice_03_07.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Practice_03_07.Controllers
{
    public class BrandController : Controller
    {

        private readonly AppDb _db;
        private readonly IWebHostEnvironment _webHostEnivornment;

        public BrandController(AppDb db, IWebHostEnvironment webHostEnivornment)
        {
            _db = db;
            _webHostEnivornment = webHostEnivornment;
        }

        public IActionResult Index()
        {
            IEnumerable<Brand> objList = _db.Brands;
            return View(objList);
        }

        [HttpGet]

        public IActionResult Upsert(int? id)
        {
            Brand brand = new Brand();

            if (id == null)
            {
                return View(brand);
            }
            else
            {
                brand = _db.Brands.Find(id);
                if (brand == null)
                {
                    return NotFound();
                }
                return View(brand);
            }
        }

         [HttpPost]
         [ValidateAntiForgeryToken]

         public IActionResult Upsert(Brand brand)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnivornment.WebRootPath;
                if (brand.Id == 0)
                {
                    string upload = webRootPath + WC.ImagePath;
                    string filename = Guid.NewGuid().ToString();
                    string extesnion = Path.GetExtension(files[0].FileName);

                    using (var filestream = new FileStream(Path.Combine(upload, filename + extesnion), FileMode.Create))
                    {
                        files[0].CopyTo(filestream);
                    }

                    brand.BrandImage = filename + extesnion;
                        
                    //create
                    _db.Brands.Add(brand);
                }
                else
                {
                    var objfromdb = _db.Brands.AsNoTracking().FirstOrDefault(e => e.Id == brand.Id);

                    if (files.Count() > 0)
                    {
                        string upload = webRootPath + WC.ImagePath;
                        string filename = Guid.NewGuid().ToString();
                        string extesnion = Path.GetExtension(files[0].FileName);

                        var oldfile = Path.Combine(upload, objfromdb.BrandImage);

                        if (System.IO.File.Exists(oldfile))
                        {
                            System.IO.File.Delete(oldfile);
                        }

                        using (var filestream = new FileStream(Path.Combine(upload, filename + extesnion), FileMode.Create))
                        {
                            files[0].CopyTo(filestream);
                        }

                        brand.BrandImage = filename + extesnion;
                    }
                    else
                    {
                        brand.BrandImage = objfromdb.BrandImage;

                    }

                    _db.Brands.Update(brand);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(brand);
        }
    }
}
