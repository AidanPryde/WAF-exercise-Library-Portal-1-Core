using System;
using System.ComponentModel.DataAnnotations;

namespace WAF_exercise_Library_Portal_1_Core_WA.Models
{
    public class RegistrationViewModel : ApplicationUserViewModel
    {
        [Required(ErrorMessage = "You must fill out the username field.")]
        [RegularExpression("^[A-Za-z0-9_-]{5,40}$", ErrorMessage = "The given username has a wrong format, or too long.")]
        public String Username { get; set; }

        [Required(ErrorMessage = "You must fill out the password field.")]
        [DataType(DataType.Password)]
        public String UserPassword { get; set; }

        [Required(ErrorMessage = "You must fill out the conformative password field.")]
        [Compare(nameof(UserPassword), ErrorMessage = "The two passwrod are not the same.")]
        [DataType(DataType.Password)]
        public String UserConfirmPassword { get; set; }
    }
}
