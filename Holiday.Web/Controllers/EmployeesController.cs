using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Holiday.Web.Data;
using Holiday.Web.Models;
using Holiday.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Holiday.Web.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;
        public EmployeesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public ActionResult Index(string sortOrder)
        {
            ViewBag.FullNameSortParam = String.IsNullOrEmpty(sortOrder) ? "fullName_desc" : "";
            ViewBag.DepSortParam = sortOrder == "dep" ? "dep_desc" : "dep";
            ViewBag.CYHNSortParam = sortOrder == "cyhn" ? "cyhn_desc" : "cyhn";

            var users = _context.Users.ToList();

            switch (sortOrder)
            {
                case "fullName_desc":
                    users = users.OrderByDescending(s => s.FullName).ToList();
                    break;
                case "dep_desc":
                    users = users.OrderByDescending(s => s.Department).ToList();
                    break;
                case "dep":
                    users = users.OrderBy(s => s.Department).ToList();
                    break;
                case "cyhn":
                    users = users.OrderByDescending(s => s.CurentYearHolidaysNumber).ToList();
                    break;
                case "cyhn_desc":
                    users = users.OrderBy(s => s.CurentYearHolidaysNumber).ToList();
                    break;
                default:
                    users = users.OrderBy(s => s.FullName).ToList();
                    break;
            }

            var viewModel = new UserViewModel();
            viewModel.Users = users;

            return View(viewModel);
        }

        public ActionResult Edit(string id)
        {
            if (_context.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().IsAdmin == false)
            {
                return Unauthorized();
            }
            var user = _context.Users.Where(x => x.Id == id).FirstOrDefault();
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: AllHolidays/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, User user)
        {
            if (_context.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().IsAdmin == false)
            {
                return Unauthorized();
            }
            try
            {
                var dbUser = _context.Users.Where(x => x.Id == id).FirstOrDefault();
                dbUser.Department = user.Department;
                dbUser.IsAdmin = user.IsAdmin;
                dbUser.CurentYearHolidaysNumber = user.CurentYearHolidaysNumber;
                _context.Update(dbUser);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}