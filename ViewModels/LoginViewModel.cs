using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;


namespace DnD_CharacterCreation.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Display(Name = "Remember me ?")]
        public bool RememberMe { get; set; }

        [Required(ErrorMessage = "Please enter the CAPTCHA code")]
        public string CaptchaInput { get; set; }

        public async Task SendEmail()
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


            using (var client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.Credentials = new NetworkCredential("robin.jardon@gmail.com", "gdwaquykrhyohxyg");
                client.EnableSsl = true;

                var mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("robin.jardon@gmail.com", "DnD_CharacterCreation");
                mailMessage.To.Add("robin.jardon@gmail.com");
                mailMessage.Subject = "Login Confirmation";
                mailMessage.IsBodyHtml = true;
                string verificationLink = "http://localhost:5000/verify?token=XYZ123";

                mailMessage.Body = $@"
                <html>
                    <body>
                        <h2>Verify your account</h2>
                        <p>Click the button below to confirm your email address:</p>
                        <a href='{verificationLink}' 
                           style='background-color: #4CAF50; color: white; padding: 14px 25px; text-decoration: none; display: inline-block; border-radius: 4px;'>
                           Verify My Email
                        </a>
                    </body>
                </html>";

                await client.SendMailAsync(mailMessage);
            }
        }

       /* [HttpGet("/verify")]
        public async Task<IActionResult> VerifyEmail(string token)
        {
            // 1. Check if the token is null
            if (string.IsNullOrEmpty(token)) return BadRequest("Invalid token.");

            // 2. Look up the user in your database who has this token
            var user = await _context.Users.FirstOrDefaultAsync(u => u.VerificationToken == token);

            if (user == null)
            {
                return View("Error", "Link expired or invalid.");
            }

            // 3. Update the user's status
            user.EmailConfirmed = true;
            user.VerificationToken = null; // Clear it so it can't be used again
            await _context.SaveChangesAsync();

            // 4. Redirect to a success page
            return View("Success");
        }*/
    }
}
