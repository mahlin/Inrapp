using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InrapporteringsPortal.Web.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Epostadress")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Kod")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Kom ihåg den här webbläsaren?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Epostadress")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Epostadress")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "PINkod")]
        public string Password { get; set; }

        [Display(Name = "Kom ihåg mig?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required (ErrorMessage = "Fältet Epostadress är obligatoriskt.")]
        [EmailAddress]
        [Display(Name = "Epostadress")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Fältet PINkod är obligatoriskt.")]
        [StringLength(100, ErrorMessage = "{0} måste vara minst {2} tecken långt.", MinimumLength = 4)]
        [DataType(DataType.Password)]
        [Display(Name = "PINkod")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Bekräfta PINkod")]
        [Compare("Password", ErrorMessage = "PINkoden och verifieringen av PINkoden stämmer inte.")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Epostadress")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} måste vara minst {2} tecken långt.", MinimumLength = 4)]
        [DataType(DataType.Password)]
        [Display(Name = "PINkod")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Bekräfta PINkod")]
        [Compare("Password", ErrorMessage = "PINkoden och verifieringen av PINkoden stämmer inte.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Epostadress")]
        public string Email { get; set; }
    }
}
