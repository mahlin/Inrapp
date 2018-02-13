using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Inrapporteringsportal.DataAccess.Repositories;
using InrapporteringsPortal.ApplicationService;
using InrapporteringsPortal.ApplicationService.Helpers;
using InrapporteringsPortal.ApplicationService.Interface;
using InrapporteringsPortal.DataAccess;
using InrapporteringsPortal.DomainModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using InrapporteringsPortal.Web.Models;

namespace InrapporteringsPortal.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private CustomIdentityResultErrorDescriber _errorDecsriber;
        private readonly IInrapporteringsPortalService _portalService;

        public AccountController()
        {
            _errorDecsriber = new CustomIdentityResultErrorDescriber();
            _portalService =
                new InrapporteringsPortalService(new PortalRepository(new ApplicationDbContext()));
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            _errorDecsriber = new CustomIdentityResultErrorDescriber();
            _portalService =
                new InrapporteringsPortalService(new PortalRepository(new ApplicationDbContext()));
        }

        public ApplicationSignInManager SignInManager
        {
            get { return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>(); }
            private set { _signInManager = value; }
        }

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                //Add this to check if the email was confirmed.
                var user = await UserManager.FindByNameAsync(model.Email);
                if (!await UserManager.IsEmailConfirmedAsync(user.Id))
                {
                    ModelState.AddModelError("", "Du behöver bekräfta din epostadress. Se mail från inrapportering@socialstyrelsen.se");
                    return View(model);
                }
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, change to shouldLockout: true
                var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe,
                    shouldLockout: false);
                switch (result)
                {
                    case SignInStatus.Success:
                        //var user = UserManager.FindByEmail(model.Email);
                        _portalService.SaveToLoginLog(user.Id, user.UserName);
                        return RedirectToLocal(returnUrl);
                    case SignInStatus.LockedOut:
                        return View("Lockout");
                    case SignInStatus.RequiresVerification:
                        if (!await UserManager.IsPhoneNumberConfirmedAsync(user.Id))
                        {
                            var phoneNumber = UserManager.GetPhoneNumberAsync(user.Id);
                            //Skicka användaren till AddPhoneNumber
                            var phoneNumberModel = new RegisterPhoneNumberViewModel()
                            {
                                Id = user.Id,
                                Number = phoneNumber.Result
                            };
                            return await this.AddPhoneNumber(phoneNumberModel);
                        }
                        else
                        {
                            return RedirectToAction("SendCode", new SendCodeViewModel
                            {
                                Providers = null,
                                ReturnUrl = returnUrl,
                                RememberMe = model.RememberMe,
                                SelectedProvider = "Phone Code",
                                UserEmail = model.Email
                            });
                        }
                    case SignInStatus.Failure:
                    default:
                        ModelState.AddModelError("", "Felaktigt användarnamn eller Pinkod.");
                        return View(model);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ErrorManager.WriteToErrorLog("AccountController", "Login", e.ToString(), e.HResult, model.Email);
                var errorModel = new CustomErrorPageModel
                {
                    Information = "Ett fel inträffade vid inloggningen",
                    ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                    ContactPhonenumber = ConfigurationManager.AppSettings["ContactPhonenumber"]
                };
                return View("CustomError", errorModel);

            }

        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe, string userEmail = "")
        {
            try
            {
                var user = UserManager.FindByEmail(userEmail);
                var phoneNumber = _portalService.HamtaAnvandaresMobilnummer(user.Id);

                if (phoneNumber == null)
                {
                    return View("Error");
                }
                else
                {
                    var model = new VerifyCodeViewModel();
                    model.PhoneNumberMasked = _portalService.MaskPhoneNumber(phoneNumber);
                    return View(model);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ErrorManager.WriteToErrorLog("AccountController", "VerifyCode", e.ToString(), e.HResult, userEmail);
                var errorModel = new CustomErrorPageModel
                {
                    Information = "Ett fel inträffade vid inloggningen",
                    ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                    ContactPhonenumber = ConfigurationManager.AppSettings["ContactPhonenumber"]
                };
                return View("CustomError", errorModel);

            }


            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe, UserEmail = userEmail});
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            try { 
                var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
                switch (result)
                {
                    case SignInStatus.Success:
                        //TODO- get userid. Logga i db
                        var user = UserManager.FindByEmail(model.UserEmail);
                        _portalService.SaveToLoginLog(user.Id, user.UserName);
                        return RedirectToLocal(model.ReturnUrl);
                    case SignInStatus.LockedOut:
                        return View("Lockout");
                    case SignInStatus.Failure:
                    default:
                        ModelState.AddModelError("", "Invalid code.");
                        return View(model);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ErrorManager.WriteToErrorLog("AccountController", "VerifyCode", e.ToString(), e.HResult, model.UserEmail);
                var errorModel = new CustomErrorPageModel
                {
                    Information = "Ett fel inträffade vid verifiering av kod.",
                    ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                    ContactPhonenumber = ConfigurationManager.AppSettings["ContactPhonenumber"]
                };
                return View("CustomError", errorModel);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            var model = new RegisterViewModel();
            //Hämta info om valbara register
            var registerInfoList = _portalService.HamtaAllRegisterInformation().ToList();
            model.RegisterList = registerInfoList;
            //model.SelectedRegisters = new List<int> {12, 7};
            //var tmpSelItem = new SelectListItem
            //{
            //    Value = "12",
            //    Text = "Maries"
            //};
            //var tmpSelItem2 = new SelectListItem
            //{
            //    Value = "7",
            //    Text = "Findus"
            //};
            //model.RegisterChoices = new List<SelectListItem>{ tmpSelItem, tmpSelItem2 };
            this.ViewBag.RegisterList = CreateRegisterDropDownList(registerInfoList);

            //Get the culture info of the language code
            CultureInfo culture = CultureInfo.GetCultureInfo("sv");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            return View(model);
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //TODO - test listbox
                    //var tmp = model.SelectedRegisters;
                    //var newRegisters = repository.Categories.Where(m => carEditViewModel.SelectedCategories.Contains(m.Id));


                    var organisation = GetOrganisationForEmailDomain(model.Email);
                    if (organisation == null)
                    {
                        ModelState.AddModelError("",
                            "Epostdomänen saknas i vårt register. Kontakta Socialstyrelsen för mer information. Support, telefonnummer: " + ConfigurationManager.AppSettings["ContactPhonenumber"]);
                    }
                    else
                    {
                        var user = new ApplicationUser {UserName = model.Email, Email = model.Email};
                        user.OrganisationId = organisation.Id;
                        user.SkapadAv = model.Email;
                        user.SkapadDatum = DateTime.Now;
                        user.AndradAv = model.Email;
                        user.AndradDatum = DateTime.Now;
                        user.Namn = model.Namn;

                        var result = await UserManager.CreateAsync(user, model.Password);
                        if (result.Succeeded)
                        {
                            await UserManager.SetTwoFactorEnabledAsync(user.Id, true);
                            //Spara valda register
                            _portalService.SparaValdaRegistersForAnvandare(user.Id, user.UserName,model.RegisterList);
                            //Verifiera epostadress
                            var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                            var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                            //TODO mail
                            await UserManager.SendEmailAsync(user.Id,
                                "Bekräfta ditt konto i Socialstyrelsens inrapporteringsportal",
                                callbackUrl);

                            return View("DisplayEmail");
                        }
                        AddErrors(result);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    ErrorManager.WriteToErrorLog("AccountController", "Register", e.ToString(), e.HResult, model.Email);
                    var errorModel = new CustomErrorPageModel
                    {
                        Information = "Ett fel inträffade vid registreringen.",
                        ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                        ContactPhonenumber = ConfigurationManager.AppSettings["ContactPhonenumber"]
                    };
                    return View("CustomError", errorModel);
                }
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        private Organisation GetOrganisationForEmailDomain(string modelEmail)
        {
                var organisation = _portalService.HamtaOrgForEmailDomain(modelEmail);
                return organisation;
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            if (result.Succeeded)
            {
                var user = UserManager.FindById(userId);
                var model = new RegisterPhoneNumberViewModel();
                model.Id = userId;
                return View("ConfirmEmail", model);
            }
           return View("Error");
        }

        // GET: AddPhoneNumber
        [AllowAnonymous]
        public ActionResult AddPhoneNumber(string id = "")
        {
            var model = new RegisterPhoneNumberViewModel();
            model.Id = id;
            return View(model);
        }

        //
        // POST: /AddPhoneNumber
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(RegisterPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var code = await UserManager.GenerateChangePhoneNumberTokenAsync(model.Id, model.Number);

                if (UserManager.SmsService != null)
                {
                    var message = new IdentityMessage
                    {
                        Destination = model.Number,
                        Body = "Välkommen till Socialstyrelsens Inrapporteringsportal. För att registera dig ange följande säkerhetskod på webbsidan: " + code

                    };
                    await UserManager.SmsService.SendAsync(message);
                }
                return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number, Id = model.Id });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ErrorManager.WriteToErrorLog("AccountController", "Register", e.ToString(), e.HResult, model.Id);
                var errorModel = new CustomErrorPageModel
                {
                    Information = "Ett fel inträffade vid registreringen.",
                    ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                    ContactPhonenumber = ConfigurationManager.AppSettings["ContactPhonenumber"]
                };
                return View("CustomError", errorModel);
            }

        }

        //
        // GET: /Manage/VerifyPhoneNumber
        [AllowAnonymous]
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber, string id)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(id, phoneNumber);
            // Send an SMS through the SMS provider to verify the phone number
            return phoneNumber == null ? View("Error") : View(new RegisterVerifyPhoneNumberViewModel { PhoneNumber = phoneNumber, Id = id });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(RegisterVerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                var result =
                    await UserManager.ChangePhoneNumberAsync(model.Id, model.PhoneNumber, model.Code);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(model.Id);
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        _portalService.SaveToLoginLog(user.Id, user.UserName);
                    }
                    return RedirectToAction("Index", "FileUpload");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ErrorManager.WriteToErrorLog("AccountController", "VerifyPhoneNumber", e.ToString(), e.HResult, User.Identity.GetUserName());
                var errorModel = new CustomErrorPageModel
                {
                    Information = "Ett fel inträffade vid verifiering av mobilnummer.",
                    ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                    ContactPhonenumber = ConfigurationManager.AppSettings["ContactPhonenumber"]
                };
                return View("CustomError", errorModel);
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Misslyckades att bekräfta mobilnummer");
            return View(model);
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                 string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                 var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                 var tmp = "Vänligen återställ ditt lösenord genom att klicka <a href=\"" + callbackUrl + "\">här</a>";
                 await UserManager.SendEmailAsync(user.Id, "Reset Password", "Vänligen återställ ditt lösenord genom att klicka <a href=\"" + callbackUrl + "\">här</a>");

                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }


        [AllowAnonymous]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                // Generate the token and send it
                if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
                {
                    return View("Error");
                }
                return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe, UserEmail = model.UserEmail });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ErrorManager.WriteToErrorLog("AccountController", "SendCode", e.ToString(), e.HResult, User.Identity.GetUserName());
                var errorModel = new CustomErrorPageModel
                {
                    Information = "Ett fel inträffade när sms-kod skulle skickas.",
                    ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                    ContactPhonenumber = ConfigurationManager.AppSettings["ContactPhonenumber"]
                };
                return View("CustomError", errorModel);
            }
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                //ModelState.AddModelError("", error);
                ModelState.AddModelError("", _errorDecsriber.LocalizeErrorMessage(error));
            }
        }

        /// <summary>  
        /// Create list for register-dropdown  
        /// </summary>  
        /// <returns>Return register for drop down list.</returns>  
        private IEnumerable<SelectListItem> CreateRegisterDropDownList(IEnumerable<RegisterInfo> registerInfoList)
        {
            SelectList lstobj = null;

            var list = registerInfoList
                .Select(p =>
                    new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = p.Namn
                    });

            // Setting.  
            lstobj = new SelectList(list, "Value", "Text");

            return lstobj;
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            //return RedirectToAction("Index", "Home");
            return RedirectToAction("Index", "FileUpload");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}