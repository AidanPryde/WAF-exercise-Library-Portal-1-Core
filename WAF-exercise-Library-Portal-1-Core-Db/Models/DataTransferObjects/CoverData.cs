using System;

namespace WAF_exercise_Library_Portal_1_Core_Db.Models.DataTransferObjects
{
    public class CoverData
    {
        public CoverData()
        {
        }

        public CoverData(Int32 id)
        {
            Id = id;
        }

        public CoverData(Int32 id, Byte[] image)
        {
            Id = id;
            Image = image;
        }

        public Int32 Id { get; set; }
        public Byte[] Image { get; set; }

        public override Boolean Equals(Object obj)
        {
            return (obj is CoverData bd) && Id == bd.Id;
        }

        public override Int32 GetHashCode()
        {
            Int32 hash = Id;

            hash = (hash * 754) + Id.GetHashCode();

            return hash;
        }
    }
}
