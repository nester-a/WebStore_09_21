using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.ViewModels.Identity
{
    public class RegisterUserViewModel
    {
        [Required]
        [Display(Name = "Имя пользователя")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Подтверждение пароля")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }
    }
}
