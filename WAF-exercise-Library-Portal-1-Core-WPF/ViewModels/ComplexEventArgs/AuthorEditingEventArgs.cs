using System;

using WAF_exercise_Library_Portal_1_Core_Db.Models.DataTransferObjects;

namespace WAF_exercise_Library_Portal_1_Core_WPF.ViewModels.ComplexEventArgs
{
    public class AuthorEditingEventArgs : EventArgs
    {
        public AuthorData AuthorData { get; private set; }
        public Int32 BookId { get; private set; }

        public AuthorEditingEventArgs(AuthorData authorData, Int32 bookId)
        {
            AuthorData = authorData;
            BookId = bookId;
        }
    }
}
