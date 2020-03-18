using Holiday.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Holiday.Web.ViewModels
{
    public class HolidayRequestCreateEditViewModel : IValidatableObject
    {
        public int? HolidayRequestId { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Required]
        [Display(Name = "Days")]
        public int Days { get; set; }

        [Required]
        public Constants.Type Type { get; set; }
        [Display(Name = "Days Left")]
        public int NoOfDaysLeft { get; set; }
      
        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            if (EndDate < StartDate)
            {
                yield return new ValidationResult("End Date must be greater than Start Date");
            }
        }
    }
}
