using System;
using System.Collections.Generic;

namespace WAF_exercise_Library_Portal_1_Core_Db.Models.DataTransferObjects
{
    public class AuthorData
    {
        public Int32 Id { get; set; }
        public String Name { get; set; }

        public IList<BookData> BookDatas { get; set; }

        public AuthorData()
        {
            Id = -1;

            BookDatas = new List<BookData>();
        }

        public AuthorData(Int32 id)
        {
            Id = id;

            BookDatas = new List<BookData>();
        }

        public AuthorData(Int32 id, String name) : this(id)
        {
            Name = name;
        }

        public override Boolean Equals(Object obj)
        {
            return (obj is AuthorData ad) && Id == ad.Id;
        }

        public override Int32 GetHashCode()
        {
            Int32 hash = Id;

            hash = (hash * 654) + Id.GetHashCode();

            return hash;
        }

        public override String ToString()
        {
            return Name;
        }
    }
}
