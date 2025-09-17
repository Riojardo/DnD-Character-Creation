using System.ComponentModel.DataAnnotations;

namespace DnD_CharacterCreation.Models
{
    public class CharacterAbility
    {
        [Key]
        public int CharacterAbilityId { get; set; }
        public int CharacterId { get; set; }
        public Character? Character { get; set; }
        public int? Strength { get; set; }
        public int? Dexterity { get; set; }
        public int? Constitution { get; set; }
        public int? Intelligence { get; set; }
        public int? Wisdom { get; set; }
        public int? Charisma { get; set; }
    }
}
