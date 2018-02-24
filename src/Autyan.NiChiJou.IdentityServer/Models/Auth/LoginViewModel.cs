using System.ComponentModel.DataAnnotations;

namespace Autyan.NiChiJou.IdentityServer.Models.Auth
{
    public class LoginViewModel
    {
        [Required]
        public string LoginName { get; set; }

        [Required]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }

    public class SignInRedirectViewModel
    {
        public string TargetUrl { get; set; }
    }

    public class UserRegisterViewModel
    {
        [Required]
        public string LoginName { get; set; }

        [Required]
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "Confirm password doesn't match, Type again !")]
        public string PasswordConfirm { get; set; }

        public string BusinessId { get; set; }
    }
}
