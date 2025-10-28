namespace DnD_CharacterCreation.ViewModels
{
    public class CharacterCreationViewModel
    {
        public List<string> ClassesNames { get; set; } = new();
        public List<string> RacesNames { get; set; } = new();
        public CharacterViewModel Character { get; set; } = new CharacterViewModel();
    }
}
