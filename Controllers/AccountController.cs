using DnD_CharacterCreation.Models;
using DnD_CharacterCreation.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace DnD_CharacterCreation.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<Users> _signInManager;
        private readonly UserManager<Users> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMemoryCache _cache;
        private readonly IConfiguration _configuration;

        public AccountController(SignInManager<Users> signInManager,
                                 UserManager<Users> userManager,
                                 RoleManager<IdentityRole> roleManager,
                                 IMemoryCache cache,
                                 IConfiguration configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _cache = cache;
            _configuration = configuration;
        }

        // ==========================================
        // LOGIN ACTIONS
        // ==========================================
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
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Email or password is incorrect");
            return View(model);
        }

        // ==========================================
        // REGISTER ACTIONS
        // ==========================================
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "Email is already registered.");
                return View(model);
            }

            try
            { 
                string validationCode = createValidationCode(model.Email, 5);
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                };
                
                _cache.Set($"RegData_{model.Email}", (Model: model, Code: validationCode), cacheOptions);

                await SendEmailWithCode(model.Email, validationCode).ConfigureAwait(false);
                return RedirectToAction("VerifyEmail", new { email = model.Email });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Verification email failed to send. Please contact support.");
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult VerifyEmail(string email)
        {
            if (string.IsNullOrEmpty(email)) return RedirectToAction("Register");
            if (!_cache.TryGetValue($"RegData_{email}", out (RegisterViewModel Model, string Code) cachedData))
            {
                return RedirectToAction("Register");
            }

            var model = new VerifyEmailViewModel
            {
                Email = email,
                Name = cachedData.Model.Name
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> VerifyEmail(VerifyEmailViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (_cache.TryGetValue($"RegData_{model.Email}", out (RegisterViewModel Model, string Code) cachedData))
            {
                if (cachedData.Code == model.CodeInput)
                {
                    var user = new Users 
                    { 
                        FullName = cachedData.Model.Name, 
                        Email = cachedData.Model.Email, 
                        UserName = cachedData.Model.Email, 
                        EmailConfirmed = true 
                    };
                    
                    var result = await _userManager.CreateAsync(user, cachedData.Model.Password);

                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, "Player");
                        
                        _cache.Remove($"RegData_{model.Email}");
                        _cache.Remove(model.Email); 

                        TempData["SuccessMessage"] = "Your account has been successfully created and verified! You can now log in.";
                        return RedirectToAction("Login");
                    }

                    foreach (var error in result.Errors) ModelState.AddModelError("", error.Description);
                    return View(model);
                }
            }

            ModelState.AddModelError("CodeInput", "The code is invalid or has expired (5 minutes limit). Please register again.");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null) return RedirectToAction("Index", "Home");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                return View("EmailConfirmedSuccess");
            }
            return View("Error");
        }

        public IActionResult ChangePassword()
        {
            return View(new ChangePasswordViewModel());
        }

        public string createValidationCode(string mailtest, int timeLimit)
        {
            int cacheValue = RandomNumberGenerator.GetInt32(100000, 1000000);
            string validationCode = cacheValue.ToString();

            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(timeLimit)
            };

            _cache.Set(mailtest, validationCode, cacheOptions);

            return validationCode;
        }

        private async Task SendEmailWithCode(string targetEmail, string code)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            string smtpEmail = _configuration["SmtpSettings:Email"] ?? "robin.jardon@gmail.com";
            string smtpPassword = _configuration["SmtpSettings:Password"] ?? "gdwaquykrhyohxyg";

            using (var client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("robin.jardon@gmail.com", "gdwaquykrhyohxyg");
                //client.Credentials = new NetworkCredential(smtpEmail, smtpPassword);
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                var mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(smtpEmail, "DnD_CharacterCreation");
                mailMessage.To.Add(targetEmail);
                mailMessage.Subject = "Your Verification Code";
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = $@"
                <html>
                    <body style='font-family: Arial, sans-serif; line-height: 1.6;'>
                        <h2 style='color: #2c3e50;'>Welcome to the Adventure!</h2>
                        <p>Thank you for joining <strong>DnD Character Creation</strong>.</p>
                        <p>Please use the following temporary security code to verify your account. This code is valid for <strong>5 minutes</strong>:</p>
                        <div style='margin: 20px 0;'>  
                           <span style='background-color: #f4f4f4; border: 1px dashed indigo; padding: 10px 20px; font-size: 24px; font-weight: bold; letter-spacing: 5px; color: indigo;'>
                               {code}
                           </span>
                        </div>
                        <p>If you did not request this code, you can safely ignore this email.</p>
                    </body>
                </html>";

                await client.SendMailAsync(mailMessage).ConfigureAwait(false);
            }
        }
    }
}