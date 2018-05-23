using System;

namespace WAF_exercise_Library_Portal_1_Core_WPF.ViewModels
{
    public class MessageEventArgs : EventArgs
    {
        public String Message { get; private set; }

        public MessageEventArgs(String message)
        {
            Message = message;
        }
    }
}
