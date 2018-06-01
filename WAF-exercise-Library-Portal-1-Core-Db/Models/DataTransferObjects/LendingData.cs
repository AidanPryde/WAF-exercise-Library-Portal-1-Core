using System;

namespace WAF_exercise_Library_Portal_1_Core_Db.Models.DataTransferObjects
{
    public class LendingData
    {
        public Int32 Id { get; set; }
        public String UserName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Byte Active { get; set; }

        public VolumeData VolumeData { get; set; }

        public LendingData()
        {
            Id = -1;
        }

        public LendingData(Int32 id, String userName, DateTime startDate, DateTime endDate, Byte active, VolumeData volumeData)
        {
            Id = id;
            UserName = userName;
            StartDate = startDate;
            EndDate = endDate;

            Active = active;

            VolumeData = volumeData;
        }

        public override Boolean Equals(Object obj)
        {
            return (obj is LendingData bd) && Id == bd.Id;
        }

        public override Int32 GetHashCode()
        {
            Int32 hash = Id;

            hash = (hash * 987) + Id.GetHashCode();

            return hash;
        }
    }
}
