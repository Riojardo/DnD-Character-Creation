using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DnD_CharacterCreation.Models
{
        public class Race
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int RaceId { get; set; }
            [Required]
            public string RaceName { get; set; } = string.Empty;
            public ICollection<SubRace>? SubRaces { get; set; }
        }
    
}
