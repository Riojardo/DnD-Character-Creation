using Microsoft.AspNetCore.Identity;

namespace DnD_CharacterCreation.Models
{
    public class Users : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;

    }
}
