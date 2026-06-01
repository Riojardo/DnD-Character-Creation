using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.Extensions.Caching.Memory;


namespace DnD_CharacterCreation.ViewModels
{    
    public class RegisterViewModel
    {
        public static string mailtest = "robin.jardon@gmail.com";

        [Required(ErrorMessage = "Name is required.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "Password required a minimum of 8 characters and a maximum of 40, please try again :/ .")]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword", ErrorMessage = "Password are not identicals.")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required.")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }

        /* [Display(Name = "Remember me ?")]
         public bool RememberMe { get; set; }*/

     

                  /*  public async Task SendEmail(string callbackUrl)
                    {

                        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


                        using (var client = new SmtpClient("smtp.gmail.com", 587))
                        {

                            client.UseDefaultCredentials = false;


                            client.Credentials = new NetworkCredential("robin.jardon@gmail.com", "gdwaquykrhyohxyg");

                            client.EnableSsl = true;
                            client.DeliveryMethod = SmtpDeliveryMethod.Network;

                            var mailMessage = new MailMessage();
                            mailMessage.From = new MailAddress("robin.jardon@gmail.com", "DnD_CharacterCreation");
                            mailMessage.To.Add("robin.jardon@gmail.com");
                            mailMessage.Subject = "Account Verification";
                            mailMessage.IsBodyHtml = true;
                            mailMessage.Body = $@"
                            <html>
                                <body style='font-family: Arial, sans-serif; line-height: 1.6;'>
                                    <h2 style='color: #2c3e50;'>Welcome to the Adventure!</h2>
                                    <p>Thank you for joining <strong>DnD Character Creation</strong>. To get started, please confirm your email address by clicking the button below:</p>
        
                                    <div style='margin: 30px 0;'>  
                                       <a href='{callbackUrl}' 
                       style='background-color: indigo; 
                              color: white; 
                              padding: 15px 25px; 
                              text-decoration: none; 
                              border-radius: 5px; 
                              font-weight: bold; 
                              display: inline-block;'>
                       Verify My Account 👍
                    </a>
                                    </div>
                                </body>
                            </html>";

                await client.SendMailAsync(mailMessage);
                        }
                    }*/



       /*              [HttpGet("/verify")]
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
                     }
*/

    }
}
