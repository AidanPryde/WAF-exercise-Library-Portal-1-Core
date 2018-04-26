using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace WAF_exercise_Library_Portal_1_Core_Db
{
    public partial class ApplicationUser : IdentityUser<int>
    {
        public ApplicationUser()
        {
            Lending = new HashSet<Lending>();
        }

        [Required]
        [MaxLength(50)]
        public String Name { get; set; }

        [Required]
        [MaxLength(250)]
        public String Address { get; set; }

        public ICollection<Lending> Lending { get; set; }
    }
}
