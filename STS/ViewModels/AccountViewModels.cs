using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace STS.ViewModels
{
    public class ProfileViewModel
    {
        [Display(Name = "EmployeePhoneNumber", ResourceType = typeof(Resources.Views.Account))]
        public string EmployeePhoneNumber { get; set; }
        [Display(Name = "EmployeeName", ResourceType = typeof(Resources.Views.Account))]
        public string EmployeeName { get; set; }
        [Display(Name = "Email", ResourceType = typeof(Resources.Views.Account))]
        public string UserEmail { get; set; }
        [Display(Name = "Location", ResourceType = typeof(Resources.Views.Account))]
        [Required(ErrorMessageResourceName = "EmployeeLocationIdRequired", ErrorMessageResourceType = typeof(Resources.Views.Account))]
        public string EmployeeLocation { get; set; }
        [Display(Name = "EmployeeDateAdded", ResourceType = typeof(Resources.Views.Account))]
        public string EmployeeDateAdded { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Views.Account))]
        [DataType(DataType.Password)]
        [Display(Name = "CurrentPassword", ResourceType = typeof(Resources.Views.Account))]
        public string OldPassword { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Views.Account))]
        [StringLength(100, ErrorMessageResourceName = "StringLength", ErrorMessageResourceType = typeof(Resources.Views.Account), MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "NewPassword", ResourceType = typeof(Resources.Views.Account))]
        public string NewPassword { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "ConfirmNewPassword", ResourceType = typeof(Resources.Views.Account))]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessageResourceName = "ConfirmPasswordCompare", ErrorMessageResourceType = typeof(Resources.Views.Account))]
        public string ConfirmPassword { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        [Display(Name = "RememberMe", ResourceType = typeof(Resources.Views.Account))]
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }
        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }
        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }
        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Views.Account))]
        [Display(Name = "Email", ResourceType = typeof(Resources.Views.Account))]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Views.Account))]
        [Display(Name = "Email", ResourceType = typeof(Resources.Views.Account))]
        [EmailAddress(ErrorMessageResourceName = "IsEmail", ErrorMessageResourceType = typeof(Resources.Views.Account))]
        public string Email { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Views.Account))]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(Resources.Views.Account))]
        public string Password { get; set; }
        [Display(Name = "RememberMe", ResourceType = typeof(Resources.Views.Account))]
        public bool RememberMe { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Views.Account))]
        [EmailAddress(ErrorMessageResourceName = "IsEmail", ErrorMessageResourceType = typeof(Resources.Views.Account))]
        [Display(Name = "Email", ResourceType = typeof(Resources.Views.Account))]
        public string Email { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Views.Account))]
        [StringLength(100, ErrorMessageResourceName = "StringLength", ErrorMessageResourceType = typeof(Resources.Views.Account), MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(Resources.Views.Account))]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "ConfirmNewPassword", ResourceType = typeof(Resources.Views.Account))]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessageResourceName = "ConfirmPasswordCompare", ErrorMessageResourceType = typeof(Resources.Views.Account))]
        public string ConfirmPassword { get; set; }
        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Views.Account))]
        [EmailAddress(ErrorMessageResourceName = "IsEmail", ErrorMessageResourceType = typeof(Resources.Views.Account))]
        [Display(Name = "Email", ResourceType = typeof(Resources.Views.Account))]
        public string Email { get; set; }
    }
}
