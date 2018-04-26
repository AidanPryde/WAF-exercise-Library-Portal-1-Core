using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WAF_exercise_Library_Portal_1_Core_Db
{
    public partial class Volume
    {
        public Volume()
        {
            Lending = new HashSet<Lending>();
        }

        [Key]
        public String Id { get; set; }

        [Required]
        [DisplayName("Book")]
        public Int32 BookId { get; set; }

        public virtual Book Book { get; set; }

        public ICollection<Lending> Lending { get; set; }
    }
}
