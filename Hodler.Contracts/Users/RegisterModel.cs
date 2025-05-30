using System.ComponentModel.DataAnnotations;

namespace Hodler.Contracts.Users
{
    public class RegisterModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^\\da-zA-Z]).{8,}$", ErrorMessage = "Password Must Have Upper & Lower Cases , Digits And Special Character")]
        [Display(Name = "Password")]
        public string Password { get; set; } = null!;

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = null!;
        [Required]
        [Display(Name = "UserName")]
        [MinLength(2)]
        public string UserName { get; set; } = null!;
        [Required]
        [Display(Name = "PhoneNumber")]
        [MinLength(7)]
        public string PhoneNumber { get; set; } = null!;
    }
}