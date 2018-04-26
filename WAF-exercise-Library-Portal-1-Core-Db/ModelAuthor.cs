using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WAF_exercise_Library_Portal_1_Core_Db
{
    public partial class Author
    {
        public Author()
        {
            BookAuthor = new HashSet<BookAuthor>();
        }

        [Key]
        public Int32 Id { get; set; }

        [Required]
        [MaxLength(50)]
        public String Name { get; set; }

        public ICollection<BookAuthor> BookAuthor { get; set; }
    }
}
