using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DnD_CharacterCreation.Models
{
    public class Class
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? ClassId { get; set; }
        public string? Name { get; set; }
        public int? HitDice { get; set; }
        public string? PrimaryAbility { get; set; }
        public string? SecondaryAbility { get; set; }
       public bool Spellcasting { get; set; } = false;

        public ICollection<Subclass> Subclasses { get; set; } = new List<Subclass>();
    }
}
