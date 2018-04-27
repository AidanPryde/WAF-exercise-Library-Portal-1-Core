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
            Lendings = new HashSet<Lending>();
        }

        [Required]
        [MaxLength(50)]
        public String Name { get; set; }

        public ICollection<Lending> Lendings { get; set; }
    }
}
