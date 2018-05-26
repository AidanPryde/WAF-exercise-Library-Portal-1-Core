using System;

namespace WAF_exercise_Library_Portal_1_Core_WPF.Persistence
{
    [Serializable]
    public class PersistenceUnavailableException : Exception
    {
        public PersistenceUnavailableException(String message) : base(message)
        {
        }

        public PersistenceUnavailableException(Exception innerException) : base("Exception occurred.", innerException)
        {
        }
    }
}
