using System.ComponentModel.DataAnnotations;

namespace DnD_CharacterCreation.Models
{
    public class CharacterSpell
    {
        [Key]
        public int CharacterSpellId { get; set; }
        public int CharacterId { get; set; }
        public Character? Character { get; set; }
        public int SpellId { get; set; }
        public Spell? Spell { get; set; }
        public bool IsPrepared { get; set; } = false;
    }
}
