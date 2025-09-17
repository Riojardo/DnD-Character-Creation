using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DnD_CharacterCreation.Models
{
    public class SubRace
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SubRaceId { get; set; }
        [Required]
        public string SubRaceName { get; set; } = string.Empty;
        public int RaceId { get; set; }
        public Race? Race { get; set; }
        public ICollection<Character>? Characters { get; set; }
    }
}
