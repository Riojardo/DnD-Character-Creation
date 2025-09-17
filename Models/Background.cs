using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DnD_CharacterCreation.Models
{
    public class Background
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? BackgroundId { get; set; }
        public string? Name { get; set; }
        public string? PrimarySkill{ get; set; }
        public string? SecondarySkill { get; set; }

    }
}
