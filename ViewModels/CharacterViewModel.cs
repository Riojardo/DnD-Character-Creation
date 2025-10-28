using DnD_CharacterCreation.Models;

namespace DnD_CharacterCreation.ViewModels
{
    public class CharacterViewModel
    {       
        public string? CharacterName { get; set; }
         public string? Alignment { get; set; }
        public string Background { get; set; }
        public List<Background> Backgrounds { get; set; } = new List<Background>();
        public int? BackgroundId { get; set; }
        public DnDLists ListsAPI { get; set; }
    }


}
