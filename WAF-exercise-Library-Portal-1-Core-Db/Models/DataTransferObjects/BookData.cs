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
        public CoverData Cover { get; set; }

        public IList<AuthorData> AuthorDatas { get; set; }
        public IList<VolumeData> VolumeDatas { get; set; }

        public BookData()
        {
            Id = -1;

            AuthorDatas = new List<AuthorData>();
            VolumeDatas = new List<VolumeData>();
        }

        public BookData(Int32 id)
        {
            Id = id;

            AuthorDatas = new List<AuthorData>();
            VolumeDatas = new List<VolumeData>();
        }

        public BookData(Int32 id, String title, Int32 publishedYear, Int64 isbn)
        {
            Id = id;
            Title = title;
            PublishedYear = publishedYear;
            Isbn = isbn;

            AuthorDatas = new List<AuthorData>();
            VolumeDatas = new List<VolumeData>();
        }

        public BookData(Int32 id, String title, Int32 publishedYear, Int64 isbn, CoverData cover)
            : this(id, title, publishedYear, isbn)
        {
            Cover = cover;
        }

        public override Boolean Equals(Object obj)
        {
            return (obj is BookData bd) && Id == bd.Id;
        }

        public override Int32 GetHashCode()
        {
            Int32 hash = Id;

            hash = (hash * 357) + Id.GetHashCode();

            return hash;
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
