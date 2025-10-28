using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DnD_CharacterCreation.Models
{
    public class CharacterSubClass
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CharacterSubClassId { get; set; }
        public int? CharacterId { get; set; }
        public Character? Character { get; set; }
        public int? ClassId { get; set; }
        public Class? Class { get; set; }
        public int? SubclassId { get; set; }
        public Subclass? SubClass { get; set; }
        public string? SubClassName {  get; set; }
    }
}
