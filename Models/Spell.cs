using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DnD_CharacterCreation.Models
{
    public class Spell
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SpellId { get; set; }
        [Required]
        public string Name { get; set; }
        public int? SpellLevel { get; set; }
        public string? SpellSchool { get; set; }
        public string? Description { get; set; }
    }
}
