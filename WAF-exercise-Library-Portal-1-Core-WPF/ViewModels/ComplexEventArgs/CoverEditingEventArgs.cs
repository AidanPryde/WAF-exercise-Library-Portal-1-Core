using System;

using WAF_exercise_Library_Portal_1_Core_Db.Models.DataTransferObjects;

namespace WAF_exercise_Library_Portal_1_Core_WPF.ViewModels.ComplexEventArgs
{
    public class CoverEditingEventArgs : EventArgs
    {
        public CoverData AuthorData { get; private set; }
        public Int32 BookId { get; private set; }

        public CoverEditingEventArgs(CoverData authorData, Int32 bookId)
        {
            AuthorData = authorData;
            BookId = bookId;
        }
    }
}
