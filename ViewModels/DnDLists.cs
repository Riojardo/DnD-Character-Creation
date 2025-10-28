using DnD_CharacterCreation.Data;
using System.Collections.Generic;
using System.Linq;
using static DnD_CharacterCreation.ViewModels.DnDLists;

namespace DnD_CharacterCreation.ViewModels
{
    public class DnDLists
    {
        private readonly AppDbContext _context;

        public List<string> ClassesNames { get; set; } = new List<string>();
        public List<string> RacesNames { get; set; } = new List<string>();
        public List<string> SpellsNames { get; set; } = new List<string>();
        public List<string> FeaturesNames { get; set; } = new List<string>();
        public List<BackgroundItem> Background { get; set; } = new List<BackgroundItem>();

        public class BackgroundItem
        {
            public int? BackgroundId { get; set; }
            public string Name { get; set; } = string.Empty;
            public string PrimarySkill { get; set; } = string.Empty;
            public string SecondarySkill { get; set; } = string.Empty;
        }

        public DnDLists(AppDbContext context)
        {
            _context = context;

            ClassesNames = _context.Class.Select(c => c.Name).ToList();
            RacesNames = _context.Race.Select(r => r.RaceName).ToList();
            Background = _context.Background
               .Select(b => new BackgroundItem
               {
                   BackgroundId = b.BackgroundId,
                   Name = b.Name,
                   PrimarySkill = b.PrimarySkill,
                   SecondarySkill = b.SecondarySkill
               })
               .ToList();
        }
    }
}