using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace DnD_CharacterCreation.Models
{
    public class CharacterSubClass
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CharacterSubClassId { get; set; }

        public int CharacterId { get; set; }        
        public int ClassId { get; set; }           
        public int SubclassId { get; set; }        
        public int CharacterClassId { get; set; } 
        public CharacterClass? CharacterClass { get; set; } 
        public Character? Character { get; set; }
        public Class? Class { get; set; }
        public Subclass? SubClass { get; set; }
        
    }
}
