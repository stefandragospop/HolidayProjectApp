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
        public string Department { get; set; }

        public bool IsAdmin { get; set; }
        public string FullName { get; set; }

        public int? CurentYearHolidaysNumber { get; set; }
    }
}
