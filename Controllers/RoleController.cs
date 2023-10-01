using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Practice_03_07.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Practice_03_07.Controllers
{
    public class RoleController : Controller
    {

        private readonly AppDb _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, AppDb db)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var roles = _db.Roles.ToList();
            return View(roles);
        }


        [HttpGet]
        public IActionResult Upsert(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                //create
                return View();
            }
            else
            {
                var objFromDB = _db.Roles.FirstOrDefault(u => u.Id == id);
                return View(objFromDB);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]


        public async Task<IActionResult> Upsert(IdentityRole role)
        {
            if(await _roleManager.RoleExistsAsync(role.Name))
            {
                TempData[SD.Error] = "Role Already Exist!";
                return RedirectToAction(nameof(Index));
            }
           
            if (string.IsNullOrEmpty(role.Id))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = role.Name });
                TempData[SD.Success] = "Role Created success";

            }
            else
            {
                var objfromdb = _db.Roles.FirstOrDefault(e => e.Id == role.Id);
                if (objfromdb == null)
                {
                    TempData[SD.Error] = "Role not found";
                    return RedirectToAction(nameof(Index));

                }

                objfromdb.Name = role.Name;
                objfromdb.NormalizedName = role.Name.ToUpper();

                var result = await _roleManager.UpdateAsync(objfromdb);
                TempData[SD.Success] = "Role Update success";
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            var objfromDB = _db.Roles.FirstOrDefault(e => e.Id == id);

            if (objfromDB == null)
            {
                TempData[SD.Error] = "Role not exist!";
                return RedirectToAction(nameof(Index));
            }

            var assignRole = _db.UserRoles.Where(e => e.RoleId == id).Count();

            if (assignRole > 0)
            {
                TempData[SD.Error] = "can not delete already role assign";
                return RedirectToAction(nameof(Index));
            }

            await _roleManager.DeleteAsync(objfromDB);
            TempData[SD.Success] = "Role Delete success";
            return RedirectToAction(nameof(Index));

        }


    }
}
