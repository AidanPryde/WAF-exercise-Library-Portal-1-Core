using System;
using System.Collections.Generic;

namespace WAF_exercise_Library_Portal_1_Core_Db.Models.DataTransferObjects
{
    public class VolumeData
    {
        public String Id { get; set; }
        public Boolean IsSortedOut { get; set; }
        public BookData BookData { get; set; }

        public ICollection<LendingData> Lendings { get; set; }

        public VolumeData()
        {

        }

        public VolumeData(String id)
        {
            Id = id;
        }

        public VolumeData(String id, Boolean isSortedOut) : this(id)
        {
            IsSortedOut = isSortedOut;
        }

        public VolumeData(String id, Boolean isSortedOut, BookData bookData) : this(id, isSortedOut)
        {
            BookData = bookData;
        }

        public override Boolean Equals(Object obj)
        {
            return (obj is VolumeData bd) && Id == bd.Id;
        }

        public override Int32 GetHashCode()
        {
            Int32 hash = Id[0] * Id[1];

            hash = (hash * 549) + Id.GetHashCode();

            return hash;
        }

        public override string ToString()
        {
            return Id;
        }
    }
}
