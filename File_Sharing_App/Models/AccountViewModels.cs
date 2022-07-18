using File_Sharing_App.Resources;
using System.ComponentModel.DataAnnotations;

namespace File_Sharing_App.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType =typeof(_SharedResource))]
        [EmailAddress(ErrorMessageResourceName = "Email", ErrorMessageResourceType = typeof(_SharedResource))]
        [Display(Name ="EmailLabel",ResourceType =typeof(_SharedResource))]
        public string Email { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(_SharedResource))]
        [Display(Name = "PasswordLabel", ResourceType = typeof(_SharedResource))]
        public string Password { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(_SharedResource))]
        [EmailAddress(ErrorMessageResourceName = "Email", ErrorMessageResourceType = typeof(_SharedResource))]
        [Display(Name = "EmailLabel", ResourceType = typeof(_SharedResource))]
        public string Email { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(_SharedResource))]
        [Display(Name = "PasswordLabel", ResourceType = typeof(_SharedResource))]
        public string Password { get; set; }
        [Compare("Password")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(_SharedResource))]
        [Display(Name = "ConfirmPasswordLabel", ResourceType = typeof(_SharedResource))]
        public string ConfirmPassword { get; set; }
        [Required]
        [Display(Name = "FristName", ResourceType = typeof(_SharedResource))]
        public string FristName { get; set; }
        [Required]
        [Display(Name = "LastName", ResourceType = typeof(_SharedResource))]
        public string LastName { get; set; }
    }
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(_SharedResource))]
        [Display(Name = "CurrentPassword", ResourceType = typeof(_SharedResource))]

        public string CurrentPassword { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(_SharedResource))]
        [Display(Name = "NewPassword", ResourceType = typeof(_SharedResource))]

        public string NewPassword { get; set; }
        [Compare("NewPassword", ErrorMessageResourceName = "ConfirmNewPasswordError", ErrorMessageResourceType = typeof(_SharedResource))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(_SharedResource))]
        [Display(Name = "ConfirmNewPassword", ResourceType = typeof(_SharedResource))]
        public string ConfirmNewPassword { get; set; }
    }
    public class AddPasswordViewModel
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(_SharedResource))]
        [Display(Name = "Password", ResourceType = typeof(_SharedResource))]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
    public class ConfirmViewModel
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string UserId { get; set; }
    }
    public class ForgetPasswordViewModel
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(_SharedResource))]
        [EmailAddress(ErrorMessageResourceName = "Email", ErrorMessageResourceType = typeof(_SharedResource))]
        [Display(Name = "EmailLabel", ResourceType = typeof(_SharedResource))]
        public string Email { get; set; }
    }
    public class VerifyResetPasswordViewModel
    {
        [Required]
        public string Token { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
    public class ResetPasswordViewModel
    {
        [Required]
        public string Token { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string NewPassword { get; set; }
        [Compare("NewPassword")]
        [Required]
        public string ConfirmNewPassword { get; set; }
    }
}
