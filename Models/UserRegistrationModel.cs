using NuGet.Protocol.Plugins;
using System.ComponentModel.DataAnnotations;

namespace IdentityByExamples.Models
{
    public class UserRegistrationModel
    {
            [Key]
            public Guid RequestId { get; set; } = Guid.NewGuid(); 

            [Required]
            public string Pseudo { get; set; }

            [Required, EmailAddress]
            public string Email { get; set; }

            [Required, DataType(DataType.Password)]
            public string Password { get; set; }

            [Required, DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "Passwords do not match")]
            public string ConfirmPassword { get; set; }
       
        
    }
}
