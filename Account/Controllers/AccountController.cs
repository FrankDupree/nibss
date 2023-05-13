using Account.Extensions;
using Account.Helpers;
using Account.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OrchardCore.Entities;
using OrchardCore.Modules;
using OrchardCore.Settings;
using OrchardCore.Users;
using OrchardCore.Users.Events;
using OrchardCore.Users.Models;
using OrchardCore.Users.Services;
using OrchardCore.Users.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using IWorkflowManager = OrchardCore.Workflows.Services.IWorkflowManager;

namespace Account.Controllers
{
    public class AccountController : NibssController
    {
        private readonly UserManager<IUser> _userManager;
        private readonly ISiteService _siteService;
        private readonly ILogger _logger;
        private readonly IUserService _userService;
        private readonly SignInManager<IUser> _signInManager;
        private readonly IEnumerable<ILoginFormEvent> _accountEvents;
        private readonly IStringLocalizer<AccountController> _s;
        private readonly IEnumerable<IPasswordRecoveryFormEvents> _passwordRecoveryFormEvents;


        public AccountController(UserManager<IUser> userManager,
            ISiteService siteService,
            ILogger<AccountController> logger,
            IStringLocalizer<AccountController> stringLocalizer,
             IUserService userService,
             SignInManager<IUser> signInManager,
                IEnumerable<ILoginFormEvent> accountEvents,
                IEnumerable<IPasswordRecoveryFormEvents> passwordRecoveryFormEvents
            )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userService = userService;
            _logger = logger;
            _siteService = siteService;
            _accountEvents = accountEvents;
            _s = stringLocalizer;
            _passwordRecoveryFormEvents = passwordRecoveryFormEvents;
        }

        [HttpGet]
        public IActionResult Register()
        {
            RegistrationViewModel applicationUserModel = new RegistrationViewModel();
            return View(applicationUserModel);
        }

        [HttpPost("account/register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistrationViewModel applicationUserModel, string returnUrl = null)
        {

            if (ModelState.IsValid && IsValidEmail(applicationUserModel.EmailAddress))
            {
                var userWithEmail = await _userManager.FindByEmailAsync(applicationUserModel.EmailAddress);

                if (userWithEmail != null)
                {
                    ModelState.AddModelError("Email", _s["A user with the same email already exists."]);
                    ViewData["msg"] = applicationUserModel.EmailAddress + " is already registered";
                    return View();
                }

                ViewData["ReturnUrl"] = returnUrl;

                var model = new RegisterViewModel
                {
                    UserName = applicationUserModel.EmailAddress,
                    Password = applicationUserModel.Password,
                    ConfirmPassword = applicationUserModel.ConfirmPassword,
                    Email = applicationUserModel.EmailAddress
                };

                var user = await _userService.CreateUserAsync(new User { UserName = model.UserName, Email = model.Email, EmailConfirmed = false }, model.Password, (key, message) => this.ModelState.AddModelError(key, message)) as User;
                if (user != null && this.ModelState.IsValid)
                {
                    //lets update the user profiles
                    var jsonObject = new JObject();
                   
                    jsonObject.Add("Organisation", HtmlEncoder.Default.Encode(applicationUserModel.Organisation));
                    jsonObject.Add("Name", HtmlEncoder.Default.Encode(applicationUserModel.Name));
                    jsonObject.Add("Sector", HtmlEncoder.Default.Encode(applicationUserModel.Sector));
                    user.Properties["UserProfile"] = jsonObject;
                    await _userManager.UpdateAsync(user);
                    //send confirmation email here
                    await this.SendEmailConfirmationTokenAsync(user, _s["Confirm your account"]);
                    ViewData["msg"] = "Registration successfull, please confirm your email.";
                    TempData["user"] = JsonConvert.SerializeObject(applicationUserModel);
                    return RedirectToAction(nameof(UserConfirmEmail));
                }
            }
            else
            {
                ViewData["ermsg"] = "Invalid User Input";
                return View();
            }

            return View(applicationUserModel);
        }

        [HttpGet("account/UserConfirmEmail")]
        [AllowAnonymous]
        public IActionResult UserConfirmEmail()
        {
            return View();
        }

        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
        }

        [HttpGet("account/ConfirmEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction(nameof(AccountController.Register), "Register");
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
            {
                return View();
            }

            return NotFound();
        }

        #region REMOTE Validation

        /// <summary>
        /// Used with jQuery Validate to check when user registers that email address not already used
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public async Task<JsonResult> CheckEmailIsUsed(string emailAddress)
        {
            //Try and get member by email typed in
            var checkEmail = await _userManager.FindByIdAsync(emailAddress);
            if (checkEmail != null)
            {
                return Json(String.Format("The email address '{0}' is already in use.", emailAddress));
            }

            return Json(true);
        }

        #endregion


        // GET: /<controller>/
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


        async Task<bool> AddConfirmEmailError(IUser user)
        {
            var registrationSettings = (await _siteService.GetSiteSettingsAsync()).As<RegistrationSettings>();
            if (registrationSettings.UsersMustValidateEmail == true && !await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError(string.Empty, _s["You must confirm your email."]);
                return true;
            }

            return false;
        }

        private async Task<IActionResult> LoggedInActionResult(IUser user, string returnUrl = null, ExternalLoginInfo info = null)
        {
            var workflowManager = HttpContext.RequestServices.GetService<IWorkflowManager>();
            if (workflowManager != null)
            {
                var input = new Dictionary<string, object>();
                input["UserName"] = user.UserName;
                input["Claims"] = info == null ? Enumerable.Empty<SerializableClaim>() : info.Principal.GetSerializableClaims();
                input["Roles"] = ((User)user).RoleNames;
                input["Provider"] = info?.LoginProvider;
                await workflowManager.TriggerEventAsync(nameof(OrchardCore.Users.Workflows.Activities.UserLoggedInEvent),
                    input: input, correlationId: ((User)user).Id.ToString());
            }
            return RedirectToLocal(returnUrl);
        }

        bool AddUserEnabledError(IUser user)
        {
            var localUser = user as User;

            if (localUser == null || !localUser.IsEnabled)
            {
                ModelState.AddModelError(String.Empty, _s["Your account is disabled. Please contact an administrator."]);
                return true;
            }

            return false;
        }

        [HttpPost("account/login")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(OrchardCore.Users.ViewModels.LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (model != null && ModelState.IsValid)
            {
                await _accountEvents.InvokeAsync((e, model, modelState) => e.LoggingInAsync(model.UserName, (key, message) => modelState.AddModelError(key, message)), model, ModelState, _logger);
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user != null)
                {
                    var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: false);
                    if (result.Succeeded && !await AddConfirmEmailError(user) && !AddUserEnabledError(user))
                    {
                        result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);

                        if (result.Succeeded)
                        {
                            await _accountEvents.InvokeAsync((e, model) => e.LoggedInAsync(model.UserName), model, _logger);
                            return await LoggedInActionResult(user, returnUrl);
                        }
                    }
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                await _accountEvents.InvokeAsync((e, model) => e.LoggingInFailedAsync(model.UserName), model, _logger);
            }
            else
            {
                ModelState.AddModelError("ermsg", "Email and password are required");
                ViewData["ermsg"] = "Email and password are required";
            }


            // If we got this far, something failed, redisplay form
            return View(model);
        }


        [HttpGet("account/login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            if (HttpContext.User != null && HttpContext.User.Identity.IsAuthenticated)
            {
                returnUrl = null;
            }

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost("account/logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return Redirect("~/");
        }

        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet("account/ResetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(string code = null)
        {
            if (!(await _siteService.GetSiteSettingsAsync()).As<ResetPasswordSettings>().AllowResetPassword)
            {
                return NotFound();
            }
            return View(new ResetPasswordViewModel { ResetToken = code });
        }

        [HttpPost("account/ResetPassword")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!(await _siteService.GetSiteSettingsAsync()).As<ResetPasswordSettings>().AllowResetPassword)
            {
                return NotFound();
            }

            await _passwordRecoveryFormEvents.InvokeAsync((e, modelState) => e.ResettingPasswordAsync((key, message) => modelState.AddModelError(key, message)), ModelState, _logger);

            if (ModelState.IsValid && await _userService.ResetPasswordAsync(model.Email, Encoding.UTF8.GetString(Convert.FromBase64String(model.ResetToken)), model.NewPassword, (key, message) => ModelState.AddModelError(key, message)))
            {
                await _passwordRecoveryFormEvents.InvokeAsync(i => i.PasswordResetAsync(), _logger);
                return RedirectToLocal(Url.Action("ResetPasswordConfirmation", "Account"));
            }

            return View(model);
        }
        [HttpGet("account/ResetPasswordConfirmation")]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction(nameof(this.Index), "Home");
        }

        //Forgot password functionality
        [HttpGet("account/ForgotPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword()
        {
            if (!(await _siteService.GetSiteSettingsAsync()).As<ResetPasswordSettings>().AllowResetPassword)
            {
                return NotFound();
            }

            return View();
        }

        [HttpPost("account/ForgotPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            Uri host = new Uri(HttpContext.Request.Headers["Origin"].ToString());

            if (!(await _siteService.GetSiteSettingsAsync()).As<ResetPasswordSettings>().AllowResetPassword)
            {
                return NotFound();
            }

            await _passwordRecoveryFormEvents.InvokeAsync((e, modelState) => e.RecoveringPasswordAsync((key, message) => modelState.AddModelError(key, message)), ModelState, _logger);


            if (ModelState.IsValid)
            {
                if (!IsValidEmail(model.UserIdentifier))
                {
                    ModelState.AddModelError("UserIdentifier", _s["Email is not valid"]);
                    return View();
                }
                var user = await _userService.GetForgotPasswordUserAsync(model.UserIdentifier) as User;
                if (user == null)

                {
                    return RedirectToLocal(Url.Action("ForgotPasswordConfirmation", "Account"));
                }
                else if (
                    (await _siteService.GetSiteSettingsAsync()).As<RegistrationSettings>().UsersMustValidateEmail
                    && !await _userManager.IsEmailConfirmedAsync(user))
                {
                    ModelState.AddModelError("UserIdentifier", _s["Email has not been verified"]);
                    return View();
                }

                user.ResetToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(user.ResetToken));
                string pain = host.ToString();
                var resetPasswordUrl = Url.Action("ResetPassword", "Account", new { code = user.ResetToken }, HttpContext.Request.Scheme, host.Authority);
                // send email with callback link
                await this.SendEmailAsync(user.Email, _s["Reset your password"], new LostPasswordViewModel() { User = user, LostPasswordUrl = resetPasswordUrl });

                await _passwordRecoveryFormEvents.InvokeAsync(i => i.PasswordRecoveredAsync(), _logger);

                return RedirectToLocal(Url.Action("ForgotPasswordConfirmation", "Account"));
            }

            // If we got this far, something failed, redisplay form
            return View();
        }
    }
}
