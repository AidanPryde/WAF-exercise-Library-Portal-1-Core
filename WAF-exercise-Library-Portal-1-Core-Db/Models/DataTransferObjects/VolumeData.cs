using System;
using System.Collections.Generic;

namespace WAF_exercise_Library_Portal_1_Core_Db.Models.DataTransferObjects
{
    public class VolumeData
    {
        public String Id { get; set; }
        public Int32 BookId { get; set; }

        public ICollection<LendingData> Lendings { get; set; }

        public override string ToString()
        {
            return Id;
        }
    }
}
