using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WAF_exercise_Library_Portal_1_Core_Db
{
    public partial class Cover
    {
        public Cover()
        {
            Book = new HashSet<Book>();
        }

        [Key]
        public Int32 Id { get; set; }

        [Required]
        public Byte[] Image { get; set; }

        public ICollection<Book> Book { get; set; }
    }
}
