using Holiday.Web.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Holiday.Web.ViewModels
{
    public class HolidayPageViewModel
    {
        public HolidayPageViewModel()
        {

        }
        [Display(Name = "Name")]
        public string EmloyeeFullName { get; set; }
        [Display(Name = "Current Year Total Days")]
        public int? EmployeeTotalNoOfDays { get; set; }
        [Display(Name = "Days left")]
        public int EmployeeTotalDaysLeft { get; set; }
        public List<HolidayRequest> Holidays { get; set; }
    }
}
