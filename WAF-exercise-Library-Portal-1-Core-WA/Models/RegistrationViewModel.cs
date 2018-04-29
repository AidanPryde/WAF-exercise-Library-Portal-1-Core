using System;
using System.ComponentModel.DataAnnotations;

namespace WAF_exercise_Library_Portal_1_Core_WA.Models
{
    public class RegistrationViewModel
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

        [Required(ErrorMessage = "You must fill out the username field.")]
        [RegularExpression("^[A-Za-z0-9_-]{5,40}$", ErrorMessage = "The given username has a wrong format, or too long.")]
        public String ApplicationUserUsername { get; set; }

        [Required(ErrorMessage = "You must fill out the password field.")]
        [DataType(DataType.Password)]
        public String ApplicationUserPassword { get; set; }

        [Required(ErrorMessage = "You must fill out the conformative password field.")]
        [Compare(nameof(ApplicationUserPassword), ErrorMessage = "The two passwrod are not the same.")]
        [DataType(DataType.Password)]
        public String ApplicationUserConfirmPassword { get; set; }
    }
}
