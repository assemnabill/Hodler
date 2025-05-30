using System.ComponentModel.DataAnnotations;

namespace Hodler.Contracts.Users
{
    public class LoginModel
    {
        [Required]
        public string EmailOrUserName { get; set; }

        [Required]
        public string Password { get; set; }

    }
}