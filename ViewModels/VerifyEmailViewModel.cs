using System.ComponentModel.DataAnnotations;
using System.Net;

namespace DnD_CharacterCreation.ViewModels
{
    public class VerifyEmailViewModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string? Email { get; set; }
       
    }

}
