using Holiday.Web.Models;
using Holiday.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Holiday.Web.Services
{
    public static class HolidayRequestService
    {
        public static async Task<HolidayPageViewModel> GetHolidayViewModel(List<HolidayRequest> holidays)
        {
            var holidayPageViewModel = new HolidayPageViewModel();
            double totalNoOfDaysTaken = 0;
            foreach (var holiday in holidays)
            {
                totalNoOfDaysTaken += CaltulateDaysBetweenTwoDates(holiday.EndDate, holiday.StartDate);
            }


            holidayPageViewModel.EmloyeeFullName = holidays.FirstOrDefault().Employee.FullName;
            holidayPageViewModel.EmployeeTotalNoOfDays = holidays.FirstOrDefault().Employee.CurentYearHolidaysNumber.HasValue ? holidays.FirstOrDefault().Employee.CurentYearHolidaysNumber.Value : 0;
            holidayPageViewModel.EmployeeTotalDaysLeft = holidayPageViewModel.EmployeeTotalNoOfDays.Value - (int)totalNoOfDaysTaken;
            holidayPageViewModel.Holidays = holidays;
            return holidayPageViewModel;

        }

        public static double CaltulateDaysBetweenTwoDates(DateTime endDate, DateTime startDate)
        {
            return (endDate - startDate).TotalDays + 1;
        }
    }
}
