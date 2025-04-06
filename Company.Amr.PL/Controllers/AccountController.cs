using Company.Amr.DAL.Models;
using Company.Amr.PL.Dtos;
using Company.Amr.PL.Helper;
using Company.Amr.PL.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Company.Amr.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMailService _mailService;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IMailService MailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mailService = MailService;
        }

        #region SignUp
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }


        [HttpPost] // GET : /Account/SignUp
        public async Task<IActionResult> SignUp(SignUpDto model) // Server Side Validation
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);

                if (user is null)
                {
                    user = await _userManager.FindByEmailAsync(model.Email);

                    if (user is null)
                    {
                        // Register
                        user = new AppUser()
                        {
                            UserName = model.UserName,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Email = model.Email,
                            IsAgree = model.IsAgree,

                        };
                        var result = await _userManager.CreateAsync(user, model.Password);
                        if (result.Succeeded)
                        {
                            // Send Email To Comfirm Email
                            return RedirectToAction(nameof(SignIn));
                        }
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }

                    }
                }
                ModelState.AddModelError("", "Invalid UserName Or Email");
            }




            return View(model);
        }

        #endregion


        #region SignIn
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SignIn(SignInDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    var flage = await _userManager.CheckPasswordAsync(user, model.Password);

                    if (flage)
                    {
                        // Sign In
                        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                        if (result.Succeeded)
                        {
                            return RedirectToAction(nameof(HomeController.Index), "Home");
                        }
                    }
                }

                ModelState.AddModelError("", "Invalid Login !");
            }
            return View(model);
        }

        #endregion


        #region SignOut
        [HttpGet]
        public new async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }

        #endregion


        #region Forget Password

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendResetPasswordUrl(ForgetPasswordDto model)
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {

                    // Generate Token

                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);


                    // Create URL

                    var url = Url.Action("ResetPassword", "Account", new { email = model.Email, token }, Request.Scheme);

                    // create Email
                    var email = new Email()
                    {
                        To = model.Email,
                        Subject = "Reset Password",
                        Body = url
                    };


                    // Send Email 
                    //var flag = EmailSettings.SendEmail(email);
                   
                    _mailService.SendEmail(email);
                        return RedirectToAction("checkYourInbox");


                    

                }

            }
            ModelState.AddModelError("", "Invalid Reset Password Operation !!");

            return View("ForgetPassword", model);

        }


        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public IActionResult checkYourInbox()
        {
            return View();
        }
        #endregion

        #region Reset Password
        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto model)
        {
            if (ModelState.IsValid)
            {
                var email = TempData["email"] as string;
                var token = TempData["token"] as string;

                if (email is null || token is null) return BadRequest("Invalid Operations");
                var user = await _userManager.FindByEmailAsync(email);
                if (user is not null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("SignIn");
                    }
                }
                ModelState.AddModelError("", "Invalid Reset Password Operation");
            }
            return View();
        }

        #endregion


    }
}




