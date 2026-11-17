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
       // public static string mailtest = "robin.jardon@gmail.com";

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


    }
}
