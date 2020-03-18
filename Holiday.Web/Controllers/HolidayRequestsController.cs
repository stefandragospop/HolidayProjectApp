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
        public async Task<IActionResult> Index()
        {
            return View(await HolidayRequestService.GetHolidayViewModel(await GetUsersHolidays()));
        }


        // GET: HolidayRequests/Create
        public async Task<IActionResult> Create()
        {
            var holidaysViewModel = await HolidayRequestService.GetHolidayViewModel(await GetUsersHolidays());

            var createViewModel = new HolidayRequestCreateEditViewModel();
            createViewModel.NoOfDaysLeft = holidaysViewModel.EmployeeTotalDaysLeft;
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
            if ((holidayRequest.EndDate - holidayRequest.StartDate).TotalDays + 1 > holidayRequest.NoOfDaysLeft)
            {
                ModelState.AddModelError(string.Empty, "Number of Holidays requested is bigger than available days.");
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

            var holidaysViewModel = HolidayRequestService.GetHolidayViewModel(GetUsersHolidays().Result).Result;

            var editViewModel = new HolidayRequestCreateEditViewModel();
            editViewModel.NoOfDaysLeft = holidaysViewModel.EmployeeTotalDaysLeft;
            editViewModel.HolidayRequestId = holidayRequest.HolidayRequestId;
            editViewModel.StartDate = holidayRequest.StartDate;
            editViewModel.EndDate = holidayRequest.EndDate;
            editViewModel.Type = holidayRequest.Type;
            
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
            if (HolidayRequestService.CaltulateDaysBetweenTwoDates(holidayRequest.EndDate, holidayRequest.StartDate) >
                (holidayRequest.NoOfDaysLeft + HolidayRequestService.CaltulateDaysBetweenTwoDates(currentHoliday.EndDate, currentHoliday.StartDate)))
            {
                ModelState.AddModelError(string.Empty, "Number of Holidays requested is bigger than available days.");
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
