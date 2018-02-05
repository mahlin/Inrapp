using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using InrapporteringsPortal.DomainModel;

namespace InrapporteringsPortal.Web.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required(ErrorMessage = "Fältet Epostadress är obligatoriskt.")]
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
        public string UserEmail { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required (ErrorMessage = "Fältet Provider är obligatoriskt.")]
        public string Provider { get; set; }

        [Required(ErrorMessage = "Fältet Kod är obligatoriskt.")]
        [Display(Name = "Kod")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Kom ihåg den här webbläsaren?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }

        public string UserEmail { get; set; }
    }


    public class RegisterVerifyPhoneNumberViewModel
    {
        [Required(ErrorMessage = "Fältet Kod är obligatoriskt.")]
        [Display(Name = "Kod")]
        public string Code { get; set; }
        public string PhoneNumber { get; set; }
        public string Id { get; set; }
    }

    public class RegisterPhoneNumberViewModel
    {
        [Required(ErrorMessage = "Fältet Mobilnummer är obligatoriskt.")]
        [Phone(ErrorMessage = "Inte ett giltigt mobilnummer")]
        [Display(Name = "Mobilnummer")]
        public string Number { get; set; }
        public string Id { get; set; }
    }

    public class ForgotViewModel
    {
        [Required(ErrorMessage = "Fältet Epostadress är obligatoriskt.")]
        [Display(Name = "Epostadress")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "Fältet Epostadress är obligatoriskt.")]
        [Display(Name = "Epostadress")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Fältet PINkod är obligatoriskt.")]
        [DataType(DataType.Password)]
        [Display(Name = "PINkod")]
        public string Password { get; set; }

        [Display(Name = "Kom ihåg mig?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Fältet Namn är obligatoriskt.")]
        [Display(Name = "Namn")]
        public string Namn { get; set; }

        [Required (ErrorMessage = "Fältet Epostadress är obligatoriskt.")]
        [EmailAddress]
        [Display(Name = "Epostadress")]
        public string Email { get; set; }

        [Required (ErrorMessage = "Fältet PINkod är obligatoriskt.")]
        [StringLength(100, ErrorMessage = "{0} måste vara minst {2} tecken långt.", MinimumLength = 4)]
        [DataType(DataType.Password)]
        [RegularExpression("([0-9]+)", ErrorMessage = "PINkoden måste vara numerisk.")]
        [Display(Name = "PINkod")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Bekräfta PINkod")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "PINkoden och verifieringen av PINkoden stämmer inte.")]
        public string ConfirmPassword { get; set; }
        public List<RegisterInfo> RegisterList { get; set; }
        //public IEnumerable<SelectListItem> RegisterChoices { get; set; }
        //private List<int> selectedRegisters;
        //public List<int> SelectedRegisters
        //{
        //    get
        //    {
        //        if (selectedRegisters == null)
        //        {
        //            selectedRegisters = RegisterList.Select(m => m.Id).ToList();
        //        }
        //        return selectedRegisters;
        //    }
        //    set { selectedRegisters = value; }
        //}
    }


    public class ResetPasswordViewModel
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
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "PINkoden och verifieringen av PINkoden stämmer inte.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required (ErrorMessage = "Fältet Epostadress är obligatoriskt.")]
        [EmailAddress]
        [Display(Name = "Epostadress")]
        public string Email { get; set; }
    }
}
