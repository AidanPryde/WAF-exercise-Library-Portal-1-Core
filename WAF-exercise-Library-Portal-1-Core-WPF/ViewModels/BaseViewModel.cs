using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using WAF_exercise_Library_Portal_1_Core_WPF.ViewModels.ComplexEventArgs;

namespace WAF_exercise_Library_Portal_1_Core_WPF.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<MessageEventArgs> MessageApplication;

        protected virtual void OnPropertyChanged([CallerMemberName] String propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void OnMessageApplication(String message)
        {
            MessageApplication?.Invoke(this, new MessageEventArgs(message));
        }
    }
}
