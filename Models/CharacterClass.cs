using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace DnD_CharacterCreation.Models
{
    public class CharacterClass
    {
        [Key]
        public int CharacterClassId { get; set; }
        public int Level { get; set; }
        public int CharacterId { get; set; }     
        public Character Character { get; set; }  
        public int ClassId { get; set; }          
        public Class Class { get; set; }         
           
    }
}