using Holiday.Web.Data;
using Holiday.Web.Models;
using Holiday.Web.Services;
using Holiday.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Holiday.Web.Controllers
{
    public class AllHolidaysController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AllHolidaysController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: AllHolidays
        public ActionResult Index(string sortOrder)
        {
            if (_context.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().IsAdmin == false)
            {
                return Unauthorized();
            }

            ViewBag.StartSortParam = String.IsNullOrEmpty(sortOrder) ? "startDate" : "";
            ViewBag.EndSortParam = sortOrder == "EndDate" ? "Enddate_desc" : "EndDate";
            ViewBag.TypeSortParam = sortOrder == "Type" ? "Type_desc" : "Type";
            ViewBag.StatusSortParam = sortOrder == "Status" ? "Status_desc" : "Status";
            ViewBag.ModifiedOnSortParam = sortOrder == "ModifiedOn" ? "ModifiedOn_desc" : "ModifiedOn";
            ViewBag.EmployeeSortParam = sortOrder == "Employee" ? "Employee_desc" : "Employee";
            ViewBag.DaysSortParam = sortOrder == "Days" ? "Days_desc" : "Days";

            var allHolidays = new AllHolidaysViewModel()
            {
                Holidays = _context.HolidayRequests.Include(x => x.Employee).Include(x => x.Approver).ToList()
            };

            switch (sortOrder)
            {
                case "startDate":
                    allHolidays.Holidays = allHolidays.Holidays.OrderBy(s => s.StartDate).ToList();
                    break;
                case "Enddate_desc":
                    allHolidays.Holidays = allHolidays.Holidays.OrderByDescending(s => s.EndDate).ToList();
                    break;
                case "EndDate":
                    allHolidays.Holidays = allHolidays.Holidays.OrderBy(s => s.EndDate).ToList();
                    break;
                case "Type_desc":
                    allHolidays.Holidays = allHolidays.Holidays.OrderByDescending(s => s.Type).ToList();
                    break;
                case "Type":
                    allHolidays.Holidays = allHolidays.Holidays.OrderBy(s => s.Type).ToList();
                    break;
                case "Status_desc":
                    allHolidays.Holidays = allHolidays.Holidays.OrderByDescending(s => s.Type).ToList();
                    break;
                case "Status":
                    allHolidays.Holidays = allHolidays.Holidays.OrderBy(s => s.Type).ToList();
                    break;
                case "ModifiedOn_desc":
                    allHolidays.Holidays = allHolidays.Holidays.OrderByDescending(s => s.ModifiedDate).ToList();
                    break;
                case "ModifiedOn":
                    allHolidays.Holidays = allHolidays.Holidays.OrderBy(s => s.ModifiedDate).ToList();
                    break;
                case "Employee_desc":
                    allHolidays.Holidays = allHolidays.Holidays.OrderByDescending(s => s.Employee.FullName).ToList();
                    break;
                case "Employee":
                    allHolidays.Holidays = allHolidays.Holidays.OrderBy(s => s.Employee.FullName).ToList();
                    break;
                case "Days_desc":
                    allHolidays.Holidays = allHolidays.Holidays.OrderByDescending(s => s.NoOfDays).ToList();
                    break;
                case "Days":
                    allHolidays.Holidays = allHolidays.Holidays.OrderBy(s => s.NoOfDays).ToList();
                    break;
                default:
                    allHolidays.Holidays = allHolidays.Holidays.OrderByDescending(s => s.StartDate).ToList();
                    break;
            }
            return View(allHolidays);
        }


        // GET: AllHolidays/Edit/5
        public ActionResult Edit(int id)
        {
            if (_context.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().IsAdmin == false)
            {
                return Unauthorized();
            }
            var holiday = _context.HolidayRequests.Include(x => x.Employee).Where(x => x.HolidayRequestId == id).FirstOrDefault();
            if (holiday == null)
            {
                return NotFound();
            }
            return View(holiday);
        }

        // POST: AllHolidays/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, HolidayRequest holidayRequest)
        {
            if (_context.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().IsAdmin == false)
            {
                return Unauthorized();
            }
            try
            {
                var dbHoliday = _context.HolidayRequests.Where(x => x.HolidayRequestId == id).FirstOrDefault();
                dbHoliday.Status = holidayRequest.Status;
                dbHoliday.Approver = _context.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
                dbHoliday.ModifiedDate = DateTime.Now;
                _context.Update(dbHoliday);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500);
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<List<HolidayRequest>> GetUsersHolidays()
        {
            return await _context.HolidayRequests.Include(x => x.Employee).Where(x => x.Employee.Email == User.Identity.Name).ToListAsync();
        }
    }
}