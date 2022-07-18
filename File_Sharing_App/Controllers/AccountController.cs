using AutoMapper;
using File_Sharing_App.Data;
using File_Sharing_App.Helper.Mail;
using File_Sharing_App.Models;
using File_Sharing_App.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Security.Claims;
using System.Text;

namespace File_Sharing_App.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> SignInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<_SharedResource> _localizer;
        private readonly IMailHelper _mailHelper;

        public AccountController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IMapper mapper,
            IStringLocalizer<_SharedResource> localizer,
            IMailHelper mailHelper
            )
        {
            this.SignInManager = signInManager;
            this.userManager = userManager;
            _mapper = mapper;
            _localizer = localizer;
            this._mailHelper = mailHelper;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl)
        {
            var existedUser =await userManager.FindByEmailAsync(model.Email);
            if (existedUser == null)
            {
                TempData["Error"] = "Invalid Email or Password";
                return View(model);
            }
            if (!existedUser.IsBlocked)
            {
                if (ModelState.IsValid)
                {
                    var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, true, true);
                    if (result.Succeeded)
                    {
                        if (!String.IsNullOrEmpty(returnUrl))
                        {
                            return LocalRedirect(returnUrl);
                        }
                        return RedirectToAction("Create", "Uploads");
                    }
                    else if (result.IsNotAllowed)
                    {
                        TempData["Error"] = _localizer["Error"].Value;
                        return View();
                    }
                }
                ModelState.AddModelError("", _localizer["loginWrong"].Value);
            }
            else
            {
                TempData["Error"] = "this account has been blocked";

            }
            return View(model); 
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FristName = model.FristName,
                    LastName = model.LastName
                };
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //create link
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var url = Url.Action("ConfirmEmail", "Account", new { token = token, userId = user.Id }, Request.Scheme);
                    //send email
                    StringBuilder body = new StringBuilder();
                    body.AppendLine("<div style='direction: ltr''>");
                    body.AppendLine("<h2>File Sharing Application : Email Confirmation </h2><br/>");
                    body.AppendFormat("to confirm your email , you should <a href='{0}'>Click Here</a>", url);
                    body.AppendLine("</div>");
                    _mailHelper.SendMail(new InputMailMessage
                    {
                        Body = body.ToString(),
                        Email = model.Email,
                        Subject = "Email Confirmation"

                    });
                    return RedirectToAction("RequireEmailConfirm");
                    //await SignInManager.SignInAsync(user, true);
                    //return RedirectToAction("Create", "Uploads");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult RequireEmailConfirm()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await SignInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult ExtarnalLogin(string provider)
        {
            var properties = SignInManager.ConfigureExternalAuthenticationProperties(provider, "Account/ExtarnalResponse");
            return Challenge(properties, provider);
        }
        public async Task<IActionResult> ExtarnalResponse()
        {
            var info = await SignInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                TempData["massege"] = "Login Failed";
                return RedirectToAction("Login");
            }
            var Extarnallogin = await SignInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
            if (!Extarnallogin.Succeeded)
            {
                var email = info.Principal.FindFirstValue(claimType: ClaimTypes.Email);
                var fname = info.Principal.FindFirstValue(claimType: ClaimTypes.GivenName);
                var lname = info.Principal.FindFirstValue(claimType: ClaimTypes.Surname);
                //create local account
                var user = new ApplicationUser
                {
                    Email = email,
                    UserName = email,
                    FristName = fname,
                    LastName = lname,
                    EmailConfirmed = true,
                };
                var createUser = await userManager.CreateAsync(user);
                if (createUser.Succeeded)
                {
                    var extarnalToLocal = await userManager.AddLoginAsync(user, info);
                    if (extarnalToLocal.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, false, info.LoginProvider);
                        return RedirectToAction("Index", "Home");

                    }
                    else
                    {
                        await userManager.DeleteAsync(user);
                    }
                }
                return RedirectToAction("Login");

            }

            if (info.Principal.HasClaim(u => u.Type == ClaimTypes.Email))
            {
                var email = info.Principal.FindFirstValue(claimType: ClaimTypes.Email);

                var existedUser = await userManager.FindByEmailAsync(email);
                if (existedUser == null)
                {
                    TempData["Error"] = "Invalid Email or Password";
                    return RedirectToAction("Login");
                }
                if (existedUser.IsBlocked)
                {
                    await SignInManager.SignOutAsync();
                    TempData["Error"] = "this account has been blocked";
                    return RedirectToAction("Login");

                }
            }
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Info()
        {
            var currentUser = await userManager.GetUserAsync(User);
            if (currentUser != null)
            {
                var model = _mapper.Map<CurrentUserViewModel>(currentUser);
                return View(model);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Info(CurrentUserViewModel model)
        {
            var currentUser = await userManager.GetUserAsync(User);
            if (ModelState.IsValid)
            {
                if (currentUser != null)
                {
                    currentUser.FristName = model.FristName;
                    currentUser.LastName = model.LastName;
                    var result = await userManager.UpdateAsync(currentUser);
                    if (result.Succeeded)
                    {

                        TempData["massege"] = _localizer["successMessage"].Value;
                        return RedirectToAction("Info");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                }
                else
                {
                    return NotFound();

                }
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            var currentUser = await userManager.GetUserAsync(User);
            if (ModelState.IsValid)
            {
                if (currentUser != null)
                {

                    var result = await userManager.ChangePasswordAsync(currentUser, model.CurrentPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        TempData["massege"] = _localizer["ChangePasswordSuccess"].Value;
                        await SignInManager.SignOutAsync();
                        return RedirectToAction("Login");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            return View("Info", _mapper.Map<CurrentUserViewModel>(currentUser));
        }
        [HttpPost]
        public async Task<IActionResult> AddPassword(AddPasswordViewModel model)
        {
            var currentUser = await userManager.GetUserAsync(User);
            if (ModelState.IsValid)
            {
                if (currentUser != null)
                {

                    var result = await userManager.AddPasswordAsync(currentUser, model.Password);
                    if (result.Succeeded)
                    {
                        TempData["massege"] = _localizer["AddPasswordSuccess"].Value;
                        await SignInManager.SignOutAsync();
                        return RedirectToAction("Login");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            return View("Info", _mapper.Map<CurrentUserViewModel>(currentUser));
        }
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(ConfirmViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByIdAsync(model.UserId);
                if (user != null)
                {
                    if (!user.EmailConfirmed)
                    {
                        var result = await userManager.ConfirmEmailAsync(user, model.Token);
                        if (result.Succeeded)
                        {
                            TempData["success"] = "Your Email Confirmed successfully";
                            return RedirectToAction("Login");
                        }
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }

                    }
                    else
                    {
                        TempData["success"] = "Your Email Already Confirmed";
                    }
                }
            }
            return View();
        }
        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var ExistUesr = await userManager.FindByEmailAsync(model.Email);
                if (ExistUesr != null)
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(ExistUesr);
                    var url = Url.Action("ResetPassword", "Account", new { token, model.Email }, Request.Scheme);
                    StringBuilder body = new StringBuilder();
                    body.AppendLine("<div style='direction: ltr''>");
                    body.AppendLine("<h2>File Sharing Application : Reset Password </h2><br/>");
                    body.AppendLine("We are sending this email, because we have recieved a reset password request to your account <br/>");
                    body.AppendFormat("to reset new password <a href='{0}'>Click this link</a>", url);
                    body.AppendLine("</div>");
                    _mailHelper.SendMail(new InputMailMessage
                    {
                        Email = model.Email,
                        Subject = "Reset Password",
                        Body = body.ToString()
                    });
                    TempData["massege"] = "If your email matched an existed account in our system , you should receive an email";
                    return RedirectToAction("Login");
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword(VerifyResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var ExistUesr = await userManager.FindByEmailAsync(model.Email);
                if (ExistUesr != null)
                {
                    var isValid = await userManager.VerifyUserTokenAsync(ExistUesr, TokenOptions.DefaultProvider, "ResetPassword", model.Token);
                    if (isValid)
                    {
                        return View();
                    }
                    else
                    {
                        TempData["Error"] = "Token is invalid";
                    }
                }
            }
            return RedirectToAction("Login");
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var ExistUesr = await userManager.FindByEmailAsync(model.Email);
                if (ExistUesr != null)
                {
                    var result = await userManager.ResetPasswordAsync(ExistUesr, model.Token, model.NewPassword);
                    if (result.Succeeded)
                    {
                        TempData["massege"] = "Reset Password completed successfully !.";
                        return RedirectToAction("Login");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }
    }
}

