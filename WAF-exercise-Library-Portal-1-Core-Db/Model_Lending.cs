using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WAF_exercise_Library_Portal_1_Core_Db
{
    public partial class Lending
    {
        [Key]
        public Int32 Id { get; set; }

        [DisplayName("Volume")]
        public String VolumeId { get; set; }

        [DisplayName("ApplicationUser")]
        public Int32 ApplicationUserId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public Byte Active { get; set; }

        public virtual Volume Volume { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
