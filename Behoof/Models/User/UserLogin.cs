using System.ComponentModel.DataAnnotations;

namespace Behoof.Models.User
{
    public class UserLogin
    {
        [Required(ErrorMessage = "Это поле обязательное")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Это поле обязательное")]
        public string? Password { get; set; }
    }
}
