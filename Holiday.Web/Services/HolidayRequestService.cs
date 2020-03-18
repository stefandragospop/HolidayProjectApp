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
        public static int CaltulateDaysBetweenTwoDates(DateTime endDate, DateTime startDate)
        {
            return (int)(endDate - startDate).TotalDays + 1;
        }

        public static int SumOfEmployeeDays(List<HolidayRequest> list)
        {
            double totalNoOfDaysTaken = 0;
            foreach (var holiday in list)
            {
                totalNoOfDaysTaken += HolidayRequestService.CaltulateDaysBetweenTwoDates(holiday.EndDate, holiday.StartDate);
            }
            return (int)totalNoOfDaysTaken;
        }
    }
}
