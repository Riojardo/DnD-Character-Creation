using DnD_CharacterCreation.Models;
using DnD_CharacterCreation.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.Mail;

namespace DnD_CharacterCreation.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<Users> _signInManager;
        private readonly UserManager<Users> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(SignInManager<Users> signInManager,
                                 UserManager<Users> userManager,
                                 RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            if (!ModelState.IsValid) return View(model);

            string storedCaptcha = HttpContext.Session.GetString("captcha");

            if (string.IsNullOrEmpty(model.CaptchaInput) ||
                !model.CaptchaInput.Equals(storedCaptcha, StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError("CaptchaInput", "Invalid CAPTCHA code. Please try again.");
                return View(model); 
            }

            HttpContext.Session.Remove("captcha");

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                model.SendEmail();
                return RedirectToAction("Index", "Home");
            }
               

            ModelState.AddModelError("", "Email or password is incorrect");


            model.SendEmail();
                
       

            return View(model);
        }

        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = new Users
            {
                FullName = model.Name,
                Email = model.Email,
                UserName = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Player");
                return RedirectToAction("Login", "Account");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult VerifyEmail()
        {

 

            return View(new VerifyEmailViewModel());

        }

        public IActionResult ChangePassword()
        {
            return View(new ChangePasswordViewModel());
        }
    }
}