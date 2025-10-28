using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DnD_CharacterCreation.Models
{
    public class Character
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CharacterId { get; set; }
        public string? UserId { get; set; } = string.Empty;
        public string? CharacterName { get; set; } = string.Empty;
        public string? Alignment { get; set; } = string.Empty;
        public int? BackgroundId { get; set; }
        public Background? Background { get; set; }
        public int? RaceId { get; set; }
        public Race? Race { get; set; }
        public int? SubRaceId { get; set; }
        public SubRace? SubRace { get; set; }
        public int? LevelTotal { get; set; }
        public int? HitPoints { get; set; }
        public int? ArmorClass { get; set; }
        public int? ProficiencyBonus => LevelTotal.HasValue ? (LevelTotal.Value - 1) / 4 + 2 : null;
        public DateTime CreationTime { get; set; } = DateTime.Now;

        public virtual Users User { get; set; }

        public ICollection<CharacterClass>? CharacterClasses { get; set; } = new List<CharacterClass>();
        public ICollection<CharacterSubClass>? CharacterSubClasses { get; set; } = new List<CharacterSubClass>();
        public ICollection<CharacterSpell>? CharacterSpells { get; set; } = new List<CharacterSpell>();
        public ICollection<CharacterFeat>? CharacterFeats { get; set; } = new List<CharacterFeat>();
    }
}
