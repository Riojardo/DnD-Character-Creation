using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DnD_CharacterCreation.Models
{
    public class Feat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FeatId {  get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

    }
}
