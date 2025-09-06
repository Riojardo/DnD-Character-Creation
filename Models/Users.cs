using Microsoft.AspNetCore.Identity;

namespace DnD_CharacterCreation.Models
{
    public class Users : IdentityUser
    {

        public String FullName { get; set; }

    }
}
