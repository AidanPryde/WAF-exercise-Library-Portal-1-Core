using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WAF_exercise_Library_Portal_1_Core_Db.Models
{
    public partial class Author
    {
        public Author()
        {
            BookAuthors = new HashSet<BookAuthor>();
        }

        [Key]
        public Int32 Id { get; set; }

        [Required]
        [MaxLength(50)]
        public String Name { get; set; }

        public ICollection<BookAuthor> BookAuthors { get; set; }
    }
}
