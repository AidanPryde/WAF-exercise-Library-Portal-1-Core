using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WAF_exercise_Library_Portal_1_Core_Db
{
    public partial class BookAuthor
    {
        [Key]
        public Int32 Id { get; set; }

        [Required]
        [DisplayName("Book")]
        public Int32 BookId { get; set; }

        [Required]
        [DisplayName("Author")]
        public Int32 AuthorId { get; set; }

        public virtual Author Author { get; set; }
        public virtual Book Book { get; set; }
    }
}
