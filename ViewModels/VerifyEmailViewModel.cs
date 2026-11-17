using System.ComponentModel.DataAnnotations;
using System.Net;

namespace DnD_CharacterCreation.ViewModels
{
    public class VerifyEmailViewModel
    {
        public string? Name { get; set; }

        [Required]
        public string Email { get; set; }
        public string? Password { get; set; }

        [Required(ErrorMessage = "Please enter your 6-digit verification code.")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "The code must be exactly 6 digits.")]
        [Display(Name = "Verification Code")]
        public string CodeInput { get; set; }
    }
}