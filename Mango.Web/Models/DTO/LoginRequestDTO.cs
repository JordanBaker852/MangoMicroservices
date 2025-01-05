using System.ComponentModel.DataAnnotations;

namespace Mango.Web.Models.DTO
{
    public class LoginRequestDTO
    {
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
