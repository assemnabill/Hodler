using System.ComponentModel.DataAnnotations;

namespace Hodler.Contracts.User.Auth
{
    public class LoginRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
