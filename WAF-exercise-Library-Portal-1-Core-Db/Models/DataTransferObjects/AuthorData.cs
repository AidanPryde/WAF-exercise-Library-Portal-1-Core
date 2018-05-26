using System;

namespace WAF_exercise_Library_Portal_1_Core_Db.Models.DataTransferObjects
{
    public class AuthorData
    {
        public Int32 Id { get; set; }
        public String Name { get; set; }

        public override bool Equals(object obj)
        {
            return (obj is AuthorData ad) && Id == ad.Id;
        }

        public override int GetHashCode()
        {
            int hash = Id;

            hash = (hash * 654) + Id.GetHashCode();

            return hash;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
