using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static Holiday.Web.Models.Constants;

namespace Holiday.Web.Models
{
    public class HolidayRequest
    {
        [Key]
        public int HolidayRequestId { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
        public Status Status { get; set; }
        [Required]
        public Constants.Type Type { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Modified on")]
        public DateTime ModifiedDate { get; set; }
        [Display(Name = "Approver")]
        public virtual User Approver { get; set; }

        public virtual User Employee { get; set; }

    }
}
