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

        public static int SumOfEmployeeDays(List<HolidayRequest> list)
        {
            double totalNoOfDaysTaken = 0;
            foreach (var holiday in list)
            {
                totalNoOfDaysTaken += GetNumberOfWorkingDays(holiday.StartDate, holiday.EndDate);
            }
            return (int)totalNoOfDaysTaken;
        }

        public static bool CheckIfUserHasHolidayRequestInPeriod(List<HolidayRequest> list, DateTime startdate, DateTime enddate)
        {
            foreach (var item in list)
            {
                if (item.StartDate == startdate)
                    return true;
                if (item.EndDate == enddate)
                    return true;
            }
            return false;
        }

        public static int GetNumberOfWorkingDays(DateTime startDate, DateTime endDate)
        {
            int days = 0;
            while (startDate <= endDate)
            {
                if (startDate.DayOfWeek != DayOfWeek.Saturday && startDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    ++days;
                }
                startDate = startDate.AddDays(1);
            }
            return days;
        }
    }
}
