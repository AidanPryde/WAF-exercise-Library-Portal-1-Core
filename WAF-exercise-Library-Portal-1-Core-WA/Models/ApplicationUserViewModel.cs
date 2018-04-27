using System;
using System.ComponentModel.DataAnnotations;

namespace WAF_exercise_Library_Portal_1_Core_WA.Models
{
    public class ApplicationUserViewModel
    {
        [Required(ErrorMessage = "You must fill out the name field.")]
        [StringLength(60, ErrorMessage = "The name can not be longer, then 60 character.")]
        public String ApplicationUserName { get; set; }

        [Required(ErrorMessage = "You mush fill out the e-mail field.")]
        [EmailAddress(ErrorMessage = "The given e-mail has a wrong format.")]
        [DataType(DataType.EmailAddress)]
        public String ApplicationUserEmail { get; set; }

        [Required(ErrorMessage = "You mush fill out the phone number field.")]
        [Phone(ErrorMessage = "The given phone number has a wrong format.")]
        [DataType(DataType.PhoneNumber)]
        public String ApplicationUserPhoneNumber { get; set; }
    }
}
