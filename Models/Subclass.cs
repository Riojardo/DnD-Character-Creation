using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DnD_CharacterCreation.Models
{
  
        public class Subclass
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int SubclassId { get; set; }
            public string? Name { get; set; }
            public string? Description { get; set; }
            public int LevelToStart { get; set; }
            public int ClassId { get; set; }
            public Class? Class { get; set; }
            public bool Spellcasting { get; set; } = false;

    }

}
