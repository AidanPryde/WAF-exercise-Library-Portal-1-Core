using System;
using System.Collections.Generic;
using System.Text;

namespace WAF_exercise_Library_Portal_1_Core_Db.Models.DataTransferObjects
{
    public class BookAuthorData
    {
        public Int32 Id;
        public BookData BookData;
        public AuthorData AuthorData;

        public BookAuthorData()
        {
            Id = -1;
        }

        public BookAuthorData(Int32 id)
        {
            Id = id;
        }

        public BookAuthorData(Int32 id, BookData bookData, AuthorData authorData) : this(id)
        {
            BookData = bookData;
            AuthorData = authorData;
        }

        public override Boolean Equals(Object obj)
        {
            return (obj is BookAuthorData bad) && Id == bad.Id;
        }

        public override Int32 GetHashCode()
        {
            Int32 hash = Id;

            hash = (hash * 9647) + Id.GetHashCode();

            return hash;
        }
    }
}
