using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Holiday.Web.Data;
using Holiday.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Holiday.Web.ViewModels;
using Holiday.Web.Services;

namespace Holiday.Web.Controllers
{
    [Authorize]
    public class HolidayRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HolidayRequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HolidayRequests
        public async Task<IActionResult> Index(string sortOrder)
        {
            ViewBag.StartSortParam = String.IsNullOrEmpty(sortOrder) ? "startDate" : "";
            ViewBag.EndSortParam = sortOrder == "EndDate" ? "Enddate_desc" : "EndDate";
            ViewBag.TypeSortParam = sortOrder == "Type" ? "Type_desc" : "Type";
            ViewBag.StatusSortParam = sortOrder == "Status" ? "Status_desc" : "Status";
            ViewBag.ModifiedOnSortParam = sortOrder == "ModifiedOn" ? "ModifiedOn_desc" : "ModifiedOn";
            ViewBag.ApproverSortParam = sortOrder == "Approver" ? "Approver_desc" : "Approver";
            ViewBag.DaysSortParam = sortOrder == "Days" ? "Days_desc" : "Days";

            var holidaysViewModel = new HolidayPageViewModel();

            holidaysViewModel.EmloyeeFullName = _context.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().FullName;
            holidaysViewModel.EmployeeTotalNoOfDays = _context.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().CurentYearHolidaysNumber;
            var holidays = await GetUsersHolidays();
            if (holidaysViewModel.EmployeeTotalNoOfDays != null) 
            {
                holidaysViewModel.EmployeeTotalDaysLeft = holidaysViewModel.EmployeeTotalNoOfDays.Value - HolidayRequestService.SumOfEmployeeDays(holidays);
            }
            else
            {
                holidaysViewModel.EmployeeTotalDaysLeft = 0;
            }
           
            holidaysViewModel.Holidays = holidays;

            switch (sortOrder)
            {
                case "startDate":
                    holidaysViewModel.Holidays = holidaysViewModel.Holidays.OrderBy(s => s.StartDate).ToList();
                    break;
                case "Enddate_desc":
                    holidaysViewModel.Holidays = holidaysViewModel.Holidays.OrderByDescending(s => s.EndDate).ToList();
                    break;
                case "EndDate":
                    holidaysViewModel.Holidays = holidaysViewModel.Holidays.OrderBy(s => s.EndDate).ToList();
                    break;
                case "Type_desc":
                    holidaysViewModel.Holidays = holidaysViewModel.Holidays.OrderByDescending(s => s.Type).ToList();
                    break;
                case "Type":
                    holidaysViewModel.Holidays = holidaysViewModel.Holidays.OrderBy(s => s.Type).ToList();
                    break;
                case "Status_desc":
                    holidaysViewModel.Holidays = holidaysViewModel.Holidays.OrderByDescending(s => s.Type).ToList();
                    break;
                case "Status":
                    holidaysViewModel.Holidays = holidaysViewModel.Holidays.OrderBy(s => s.Type).ToList();
                    break;
                case "ModifiedOn_desc":
                    holidaysViewModel.Holidays = holidaysViewModel.Holidays.OrderByDescending(s => s.ModifiedDate).ToList();
                    break;
                case "ModifiedOn":
                    holidaysViewModel.Holidays = holidaysViewModel.Holidays.OrderBy(s => s.ModifiedDate).ToList();
                    break;
                case "Approver_desc":
                    holidaysViewModel.Holidays = holidaysViewModel.Holidays.OrderByDescending(s => s.Approver.FullName).ToList();
                    break;
                case "Approver":
                    holidaysViewModel.Holidays = holidaysViewModel.Holidays.OrderBy(s => s.Approver.FullName).ToList();
                    break;
                case "Days_desc":
                    holidaysViewModel.Holidays = holidaysViewModel.Holidays.OrderByDescending(s => s.NoOfDays).ToList();
                    break;
                case "Days":
                    holidaysViewModel.Holidays = holidaysViewModel.Holidays.OrderBy(s => s.NoOfDays).ToList();
                    break;
                default:
                    holidaysViewModel.Holidays = holidaysViewModel.Holidays.OrderByDescending(s => s.StartDate).ToList();
                    break;
            }
            return View(holidaysViewModel);
        }


        // GET: HolidayRequests/Create
        public async Task<IActionResult> Create()
        {
            var createViewModel = new HolidayRequestCreateEditViewModel();
            var user = await _context.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefaultAsync();
            createViewModel.NoOfDaysLeft = user.CurentYearHolidaysNumber.Value - HolidayRequestService.SumOfEmployeeDays(await GetUsersHolidays());
            createViewModel.StartDate = DateTime.Now.Date;
            createViewModel.EndDate = DateTime.Now.Date;

            return View(createViewModel);
        }

        // POST: HolidayRequests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HolidayRequestId,StartDate,EndDate,Type,NoOfDaysLeft")] HolidayRequestCreateEditViewModel holidayRequest)
        {
            if (HolidayRequestService.GetNumberOfWorkingDays(holidayRequest.StartDate, holidayRequest.EndDate) >
                (holidayRequest.NoOfDaysLeft))
            {
                ModelState.AddModelError(string.Empty, "Number of Holidays requested is bigger than available days.");
            }
            var usersHolidays = await GetUsersHolidays();
            if (HolidayRequestService.CheckIfUserHasHolidayRequestInPeriod(usersHolidays, holidayRequest.StartDate, holidayRequest.EndDate))
            {
                ModelState.AddModelError(string.Empty, $"There are holidays between {holidayRequest.StartDate.ToString("dd/MM/yyyy")} and {holidayRequest.EndDate.ToString("dd/MM/yyyy")}. Please re-enter");
            }
            if (ModelState.IsValid)
            {
                var dbHolidayRequest = new HolidayRequest();
                dbHolidayRequest.Employee = _context.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
                dbHolidayRequest.ModifiedDate = DateTime.Now;
                dbHolidayRequest.Status = Constants.Status.Pending;
                dbHolidayRequest.StartDate = holidayRequest.StartDate;
                dbHolidayRequest.EndDate = holidayRequest.EndDate;
                dbHolidayRequest.Type = holidayRequest.Type;
                dbHolidayRequest.NoOfDays = HolidayRequestService.GetNumberOfWorkingDays(holidayRequest.StartDate, holidayRequest.EndDate);
                _context.Add(dbHolidayRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(holidayRequest);
        }

        // GET: HolidayRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var holidayRequest = await _context.HolidayRequests.Include(x => x.Employee).Where(x => x.HolidayRequestId == id).FirstOrDefaultAsync();
            if (holidayRequest.Employee.UserName != User.Identity.Name)
            {
                return Unauthorized();
            }
            if (holidayRequest == null)
            {
                return NotFound();
            }

            var user = await _context.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefaultAsync();
            var editViewModel = new HolidayRequestCreateEditViewModel();

            editViewModel.NoOfDaysLeft = user.CurentYearHolidaysNumber.Value - HolidayRequestService.SumOfEmployeeDays(await GetUsersHolidays());
            editViewModel.HolidayRequestId = holidayRequest.HolidayRequestId;
            editViewModel.StartDate = holidayRequest.StartDate;
            editViewModel.EndDate = holidayRequest.EndDate;
            editViewModel.Days = holidayRequest.NoOfDays;

            return View(editViewModel);
        }

        // POST: HolidayRequests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HolidayRequestId,StartDate,EndDate,Type,NoOfDaysLeft")] HolidayRequestCreateEditViewModel holidayRequest)
        {
            if (id != holidayRequest.HolidayRequestId)
            {
                return NotFound();
            }

            var currentHoliday = await _context.HolidayRequests.Where(x => x.HolidayRequestId == id).FirstOrDefaultAsync();
            if (HolidayRequestService.GetNumberOfWorkingDays(holidayRequest.StartDate, holidayRequest.EndDate) >
                (holidayRequest.NoOfDaysLeft + currentHoliday.NoOfDays))
            {
                ModelState.AddModelError(string.Empty, "Number of Holidays requested is bigger than available days.");
                holidayRequest.Days = HolidayRequestService.GetNumberOfWorkingDays(holidayRequest.StartDate, holidayRequest.EndDate);
            }
            var usersHolidays = await GetUsersHolidays();
            if (HolidayRequestService.CheckIfUserHasHolidayRequestInPeriod(usersHolidays, holidayRequest.StartDate, holidayRequest.EndDate))
            {
                ModelState.AddModelError(string.Empty, $"There are holidays between {holidayRequest.StartDate.ToString("dd/MM/yyyy")} and {holidayRequest.EndDate.ToString("dd/MM/yyyy")}. Please re-enter");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    currentHoliday.ModifiedDate = DateTime.Now;
                    currentHoliday.Status = Constants.Status.Pending;
                    currentHoliday.StartDate = holidayRequest.StartDate;
                    currentHoliday.EndDate = holidayRequest.EndDate;
                    currentHoliday.Type = holidayRequest.Type;
                    currentHoliday.NoOfDays = HolidayRequestService.GetNumberOfWorkingDays(holidayRequest.StartDate, holidayRequest.EndDate);
                    _context.Update(currentHoliday);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HolidayRequestExists(currentHoliday.HolidayRequestId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(holidayRequest);
        }

        // GET: HolidayRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var holidayRequest = await _context.HolidayRequests.Include(x => x.Employee).Include(x => x.Approver).Where(x => x.HolidayRequestId == id).FirstOrDefaultAsync();
            if (holidayRequest.Employee.UserName != User.Identity.Name)
            {
                return Unauthorized();
            }
            if (holidayRequest == null)
            {
                return NotFound();
            }

            return View(holidayRequest);
        }

        // POST: HolidayRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var holidayRequest = await _context.HolidayRequests.FindAsync(id);
            _context.HolidayRequests.Remove(holidayRequest);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HolidayRequestExists(int id)
        {
            return _context.HolidayRequests.Any(e => e.HolidayRequestId == id);
        }

        private async Task<List<HolidayRequest>> GetUsersHolidays()
        {
            return await _context.HolidayRequests.Include(x => x.Employee).Where(x => x.Employee.Email == User.Identity.Name).ToListAsync();
        }


    }
}
