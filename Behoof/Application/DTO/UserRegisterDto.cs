using System.ComponentModel.DataAnnotations;

namespace Behoof.Application.DTO
{
    public class UserRegisterDto
    {
        [Required(ErrorMessage = "Это поле обязательное")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Это поле обязательное")]
        public string? Login { get; set; }

        [Required(ErrorMessage = "Это поле обязательное")]
        [Compare("ComparePassword", ErrorMessage = "Пароли не совпадают")]
        public string? Password { get; set; }
        [Required(ErrorMessage = "Это поле обязательное")]
        public string? ComparePassword { get; set; }
    }
}
