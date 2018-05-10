﻿using System.ComponentModel.DataAnnotations;

namespace WAF_exercise_Library_Portal_1_Core_Db.DataTransferObjects
{
    public class Login
    {
        [Required] public string UserName { get; set; }

        [Required] public string Password { get; set; }
    }
}
