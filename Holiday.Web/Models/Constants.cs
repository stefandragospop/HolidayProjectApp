﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Holiday.Web.Models
{
    public class Constants
    {
        public enum Status
        {

            Pending,
            Accepted,
            Declined
        }

        public enum Type
        {
            Medical,
            Paid,
            Training,
            [Display(Name = "Blood Donation")]
            BloodDonation,
            Legal
        }

        public static int DefaultNumberOfHolidaysPerYear { get; set; } = 21;
    }
}
