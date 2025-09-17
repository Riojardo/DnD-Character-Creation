using System.ComponentModel.DataAnnotations;

namespace DnD_CharacterCreation.Models
{
    public class CharacterFeat
    {
        [Key]
        public int CharacterFeatId { get; set; }
        public int CharacterId { get; set; }
        public Character Character { get; set; }
        public int FeatId { get; set; }
        public Feat Feat { get; set; }
    }
}
