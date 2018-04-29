using System;
using System.ComponentModel.DataAnnotations;

namespace WAF_exercise_Library_Portal_1_Core_WA.Models
{
    public class LendingViewModel
    {
        public String VolumeId { get; set; }

        [Required(ErrorMessage = "You mush fill out the start date field.")]
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "You mush fill out the end date field.")]
        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }
    }
}
