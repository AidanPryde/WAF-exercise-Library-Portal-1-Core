using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WAF_exercise_Library_Portal_1_Core_Db.Models
{
    public partial class Volume
    {
        public Volume()
        {
            Lendings = new HashSet<Lending>();
        }

        [Key]
        public String Id { get; set; }

        public Boolean IsSordtedOut { get; set; }

        [Required]
        [DisplayName("Book")]
        public Int32 BookId { get; set; }

        public virtual Book Book { get; set; }

        public ICollection<Lending> Lendings { get; set; }

        public IEnumerable<Lending> GetRelevantLendings()
        {
            DateTime now = DateTime.UtcNow;

            return Lendings.Where(l => (l.StartDate > now || l.EndDate > now)).OrderBy(l => l.StartDate);
        }
    }
}
