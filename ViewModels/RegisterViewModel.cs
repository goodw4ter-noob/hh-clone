using hh_clone.DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace hh_clone.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email не должен быть пустым")]
        [EmailAddress(ErrorMessage = "Некорректный формат электронной почты")]
        public string? Email { get; set; }
		
        [Required(ErrorMessage = "Пароль не должен быть пустым")]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[!@#$%^&*-]).{10,}$",
            ErrorMessage ="Паполь слишком простой")]
		public string? Password { get; set; }
    }
}
