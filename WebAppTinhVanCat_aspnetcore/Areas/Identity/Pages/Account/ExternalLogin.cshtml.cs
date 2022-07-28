using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using WebAppTinhVanCat_aspnetcore.Models;

namespace WebAppTinhVanCat_aspnetcore.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ExternalLoginModel : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ExternalLoginModel> _logger;

        public ExternalLoginModel(
            SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager,
            ILogger<ExternalLoginModel> logger,
            IEmailSender emailSender)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ProviderDisplayName { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public IActionResult OnGetAsync()
        {
            return RedirectToPage("./Login");
        }

        public IActionResult OnPost(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
        {
             
            
            returnUrl = returnUrl ?? Url.Content("~/");
            if (remoteError != null)
            {
                ErrorMessage = $"Lỗi từ dịch vụ ngoài: {remoteError}";
                return RedirectToPage("./Login", new {ReturnUrl = returnUrl });
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Không lấy được thông tin từ dịch vụ ngoài.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor : true);
            if (result.Succeeded)
            {
                _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
                return LocalRedirect(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return RedirectToPage("./Lockout");
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ReturnUrl = returnUrl;
                ProviderDisplayName = info.ProviderDisplayName;
                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                {
                    Input = new InputModel
                    {
                        Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                    };
                }
                return Page();
            }
        }

        public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            // Get the information about the user from the external login provider
            var info = await _signInManager.GetExternalLoginInfoAsync(); //thông tin dịch vụ ngoài
            if (info == null)
            {
                ErrorMessage = "Lỗi lấy thông tin từ dịch vụ ngoài.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            if (ModelState.IsValid)
            {

                var registeredUser = await _userManager.FindByEmailAsync(Input.Email); //lấy người dùng cần đăng ký ứng với email submit
                string externalEmail = null;
                AppUser externalEmailUser = null;

                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email)) // lấy email ngoài
                {
                    externalEmail = info.Principal.FindFirstValue(ClaimTypes.Email);  //email ngoài
                }

                if(externalEmail != null)// lấy email người dung ứng với email ngoài
                {
                    externalEmailUser = await _userManager.FindByEmailAsync(externalEmail); 
                }

                //-------------- dk

                if((registeredUser != null) && (externalEmailUser != null))// th1:có tk trên hệ thống 
                {
                    if(registeredUser.Id == externalEmailUser.Id)
                    {
                        // lien ket tai khoan dang nhap
                        var resultLink = await _userManager.AddLoginAsync(registeredUser, info);
                        if(resultLink.Succeeded)
                        {
                           await _signInManager.SignInAsync(registeredUser,isPersistent : false );
                           return LocalRedirect(returnUrl);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(String.Empty, "Không liên kết được tài khoản, hay sử dụng Email khác !");
                        return Page();
                    }

                }
                
                if((externalEmailUser != null) && (registeredUser == null)){
                    ModelState.AddModelError(String.Empty, "Không hổ trợ tạo tài khoản mới có Email khác với dịch vụ ngoài !");
                    return Page();
                }


                if((externalEmailUser == null)&&(externalEmail == Input.Email))
                {
                    var newUser = new AppUser(){
                        UserName = externalEmail,
                        Email = externalEmail

                    };

                    var resultNewUser  = await  _userManager.CreateAsync(newUser);
                    if(resultNewUser.Succeeded)
                    {
                        await _userManager.AddLoginAsync(newUser , info);
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                        await _userManager.ConfirmEmailAsync(newUser,code);

                        await _signInManager.SignInAsync(newUser, isPersistent: false);
                        return LocalRedirect(returnUrl);

                    }
                    else
                    {
                        ModelState.AddModelError(String.Empty, "Không tạo được tài khoản mới!");
                        return Page();
                    }

                }

                




                var user = new AppUser { UserName = Input.Email, Email = Input.Email };

                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);

                        var userId = await _userManager.GetUserIdAsync(user);
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { area = "Identity", userId = userId, code = code },
                            protocol: Request.Scheme);

                        await _emailSender.SendEmailAsync(Input.Email, "Xác thực Email",
                        $"Bạn vừa đăng ký tài khoản trên XXX ?.<br/> Vui lòng xác tài khoản <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>tại đây</a>. Để đăng nhập.");

                        // If account confirmation is required, we need to show the link if we don't have a real email sender
                        if (_userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            return RedirectToPage("./RegisterConfirmation", new { Email = Input.Email });
                        }

                        await _signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider);

                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            ProviderDisplayName = info.ProviderDisplayName;
            ReturnUrl = returnUrl;
            return Page();
        }
    }
}
