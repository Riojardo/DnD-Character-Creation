using DnD_CharacterCreation.Models;

namespace DnD_CharacterCreation.ViewModels
{
    public class CharacterViewModel
    {       
        public string? CharacterName { get; set; }
         public string? Alignment { get; set; }
        public string? Background { get; set; }
        public int? BackgroundId { get; set; }
        public int? Classe_First { get; set; }
        public int? Levelclasss_First { get; set; }
        public int? Classe_Second { get; set; }
        public int? Levelclasss_Second { get; set; }
        public int? Classe_Third { get; set; }
        public int? Levelclasss_Third { get; set; }
        public int? Subclasse_First { get; set; }
        public int? Subclasse_Second { get; set; }
        public int? Subclasse_Third { get; set; }

        public List<Background> Backgrounds { get; set; } = new();

        public DnDLists ListsAPI { get; set; }
    }

}
