using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;
using System.ComponentModel;

namespace WAF_exercise_Library_Portal_1_Core_Db
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
        /*[Index("INDEX_AUTHOR_NAME", IsClustered = false, IsUnique = true)]*/
        public String Name { get; set; }

        public ICollection<BookAuthor> BookAuthors { get; set; }
    }
}
