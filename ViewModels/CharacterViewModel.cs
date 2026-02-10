using DnD_CharacterCreation.Models;

namespace DnD_CharacterCreation.ViewModels
{
    public class CharacterViewModel
    {       
        public string? CharacterName { get; set; }
        public string? Alignment { get; set; }
        public string? Background { get; set; }
        public int? BackgroundId { get; set; }
        public int? Species { get; set; }
        public int? Subspecies { get; set; }
        public int? Classe_First { get; set; }
        public int? Levelclasss_First { get; set; }
        public int? Classe_Second { get; set; }
        public int? Levelclasss_Second { get; set; }
        public int? Classe_Third { get; set; }
        public int? Levelclasss_Third { get; set; }
        public int? Subclasse_First { get; set; }
        public int? Subclasse_Second { get; set; }
        public int? Subclasse_Third { get; set; }
        public string? total_STR { get; set; }
        public string? total_DEX { get; set; }
        public string? total_CON { get; set; }
        public string? total_INT { get; set; }
        public string? total_WIS { get; set; }
        public string? total_CHA { get; set; }
        public bool Acrobatics { get; set; } = false;
        public bool AnimalHandling { get; set; } = false;
        public bool Arcana { get; set; } = false;
        public bool Athletics { get; set; } = false;
        public bool Deception { get; set; } = false;
        public bool History { get; set; } = false;
        public bool Insight { get; set; } = false;
        public bool Intimidation { get; set; } = false;
        public bool Investigation { get; set; } = false;
        public bool Medicine { get; set; } = false;
        public bool Nature { get; set; } = false;
        public bool Perception { get; set; } = false;
        public bool Performance { get; set; } = false;
        public bool Persuasion { get; set; } = false;
        public bool Religion { get; set; } = false;
        public bool SleightOfHand { get; set; } = false;
        public bool Stealth { get; set; } = false;
        public bool Survival { get; set; } = false;

        public List<Background> Backgrounds { get; set; } = new();

        public DnDLists ListsAPI { get; set; }
    }

}
