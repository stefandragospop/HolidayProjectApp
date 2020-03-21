using Holiday.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static Holiday.Web.Models.Constants;

namespace Holiday.Web.Models
{
    public class HolidayRequest : IValidatableObject
    {
        [Key]
        public int HolidayRequestId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Days")]
        public int NoOfDays { get; set; }

        public Constants.Status Status { get; set; }

        [Required]
        public Constants.Type Type { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Modified on")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime ModifiedDate { get; set; }

        [Display(Name = "Approver")]
        public virtual User Approver { get; set; }

        public virtual User Employee { get; set; }

        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            if (EndDate < StartDate)
            {
                yield return new ValidationResult("End Date must be greater than Start Date");
            }
        }
    }
}
