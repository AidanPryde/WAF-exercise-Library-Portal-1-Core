using System;
using System.Collections.Generic;

namespace WAF_exercise_Library_Portal_1_Core_Db.Models.DataTransferObjects
{
    public class CoverData
    {
        public Int32 Id { get; set; }
        public Byte[] Image { get; set; }

        public IList<BookData> BookDatas { get; set; }

        public CoverData()
        {
            Id = -1;

            BookDatas = new List<BookData>();
        }

        public CoverData(Int32 id)
        {
            Id = id;

            BookDatas = new List<BookData>();
        }

        public CoverData(Int32 id, Byte[] image) : this(id)
        {
            Image = image;
        }

        public CoverData(Int32 id, Byte[] image, BookData bookData) : this(id, image)
        {
            BookDatas.Add(bookData);
        }

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
