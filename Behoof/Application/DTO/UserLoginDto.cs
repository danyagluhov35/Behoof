using System.ComponentModel.DataAnnotations;

namespace Behoof.Application.DTO
{
    public class UserLoginDto
    {
        [Required(ErrorMessage = "Это поле обязательное")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Это поле обязательное")]
        public string? Password { get; set; }
    }
}
