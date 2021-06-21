using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BusinessCardWebsite.Models
{
    public class UserModel
    {
        [Display(Name = "Логин")]
        [Required(ErrorMessage = "Введите логин")]
        public string login { get; set; }

        [Display(Name = "Пароль")]
        [DataType("Password")]
        [Required(ErrorMessage = "Введите пароль")]
        public string password { get; set; }
        public bool authed { get; set; }
        public string message { get; set; }

        public string session_value { get; set; }
    }
}