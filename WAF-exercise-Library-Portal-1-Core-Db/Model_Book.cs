using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WAF_exercise_Library_Portal_1_Core_Db
{
    public partial class Book
    {
        public Book()
        {
            BookAuthors = new HashSet<BookAuthor>();
            Volumes = new HashSet<Volume>();
        }

        [Key]
        public Int32 Id { get; set; }

        [Required]
        public String Title { get; set; }

        [Required]
        public Int32 PublishedYear { get; set; }

        [Required]
        public Int64 Isbn { get; set; }

        [DisplayName("Cover")]
        public Int32? CoverId { get; set; }

        public virtual Cover Cover { get; set; }

        public ICollection<BookAuthor> BookAuthors { get; set; }
        public ICollection<Volume> Volumes { get; set; }

        public String GetFristAuthorName()
        {
            return BookAuthors.First().Author.Name;
        }
    }
}
