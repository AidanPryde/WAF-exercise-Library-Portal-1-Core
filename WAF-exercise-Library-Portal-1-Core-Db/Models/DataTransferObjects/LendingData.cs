using System;
using System.Collections.Generic;
using System.Text;

namespace WAF_exercise_Library_Portal_1_Core_Db.Models.DataTransferObjects
{
    public class LendingData
    {
        public Int32 Id { get; set; }
        public String VolumeId { get; set; }
        public String UserName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Byte Active { get; set; }
    }
}
