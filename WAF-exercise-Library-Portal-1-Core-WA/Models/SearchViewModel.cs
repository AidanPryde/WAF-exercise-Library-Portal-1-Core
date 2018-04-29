using System;
using System.ComponentModel.DataAnnotations;

namespace WAF_exercise_Library_Portal_1_Core_WA.Models
{
    public class SearchViewModel
    {
        [RegularExpression("^[A-Za-z0-9_:-]{0,40}$", ErrorMessage = "The given title has a wrong format, or too long.")]
        public String Title { get; set; }

        [RegularExpression("^[A-Za-z-]{0,40}$", ErrorMessage = "The given title has a wrong format, or too long.")]
        public String Author { get; set; }
    }
}