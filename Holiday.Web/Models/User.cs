using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Holiday.Web.Models
{
    public class User : IdentityUser
    {
        [Display(Name = "Department")]
        public string Department { get; set; }
        [Display(Name = "Is Admin?")]
        public bool IsAdmin { get; set; }
        [Display(Name = "Full Name")]
        public string FullName { get; set; }
        [Display(Name = "Current Year Holidyas Number")]
        public int? CurentYearHolidaysNumber { get; set; }
    }
}
