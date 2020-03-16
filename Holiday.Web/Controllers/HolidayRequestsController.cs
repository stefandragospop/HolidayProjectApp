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
            return View(await _context.HolidayRequests.Where(x=>x.Employee.Email == User.Identity.Name).ToListAsync());
        }

        // GET: HolidayRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var holidayRequest = await _context.HolidayRequests.Include(x => x.Employee).Include(x=>x.Approver).Where(x => x.HolidayRequestId == id).FirstOrDefaultAsync();
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

        // GET: HolidayRequests/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HolidayRequests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HolidayRequestId,StartDate,EndDate,Status,Type")] HolidayRequest holidayRequest)
        {
            if (ModelState.IsValid)
            {
                holidayRequest.Employee = _context.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
                holidayRequest.ModifiedDate = DateTime.Now;
                holidayRequest.Status = Constants.Status.Pending;
                _context.Add(holidayRequest);
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

            var holidayRequest = await _context.HolidayRequests.Include(x=>x.Employee).Where(x=>x.HolidayRequestId == id).FirstOrDefaultAsync();
            if(holidayRequest.Employee.UserName != User.Identity.Name)
            {
                return Unauthorized();
            }
            if (holidayRequest == null)
            {
                return NotFound();
            }
            return View(holidayRequest);
        }

        // POST: HolidayRequests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HolidayRequestId,StartDate,EndDate,Status,Type")] HolidayRequest holidayRequest)
        {
            if (id != holidayRequest.HolidayRequestId)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    holidayRequest.Employee = _context.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
                    holidayRequest.Status = Constants.Status.Pending;
                    _context.Update(holidayRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HolidayRequestExists(holidayRequest.HolidayRequestId))
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

            var holidayRequest = await _context.HolidayRequests.Include(x => x.Employee).Include(x=>x.Approver).Where(x => x.HolidayRequestId == id).FirstOrDefaultAsync();
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
    }
}
