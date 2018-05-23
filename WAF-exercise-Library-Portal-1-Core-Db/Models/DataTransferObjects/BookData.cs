using System;
using System.Collections.Generic;

namespace WAF_exercise_Library_Portal_1_Core_Db.Models.DataTransferObjects
{
    public class BookData
    {
        public Int32 Id { get; set; }
        public String Title { get; set; }
        public Int32 PublishedYear { get; set; }
        public Int64 Isbn { get; set; }
        public Byte[] Image { get; set; }

        public IList<AuthorData> Authors { get; set; }
        public IList<VolumeData> Volumes { get; set; }

        public override Boolean Equals(Object obj)
        {
            return (obj is BookData bd) && Id == bd.Id;
        }

        public override int GetHashCode()
        {
            int hash = Id;

            hash = (hash * 357) + Isbn.GetHashCode();

            return hash;
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
