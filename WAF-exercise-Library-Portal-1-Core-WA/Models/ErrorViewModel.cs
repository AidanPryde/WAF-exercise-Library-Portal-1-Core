using System;

namespace WAF_exercise_Library_Portal_1_Core_WA.Models
{
    public class ErrorViewModel
    {
        public String RequestId { get; set; }

        public Boolean ShowRequestId => !String.IsNullOrEmpty(RequestId);
    }
}