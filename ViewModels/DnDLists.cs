using DnD_CharacterCreation.Data;
using System.Collections.Generic;
using System.Linq;
using static DnD_CharacterCreation.ViewModels.DnDLists;

namespace DnD_CharacterCreation.ViewModels
{
    public class DnDLists
    {
        private readonly AppDbContext _context;

        public List<KeyValuePair<int?, string>> ClassesNames { get; set; } = new();
        public class SubClassInfo
        {
            public int? SubclassId { get; set; }
            public string SubName { get; set; } = string.Empty;          
            public int LevelToStart { get; set; }
            public int? ClassId { get; set; }
        }
        public Dictionary<int, List<SubClassInfo>> SubClassesNames { get; set; } = new();
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

           ClassesNames = _context.Class
                        .Select(c => new KeyValuePair<int?, string>(c.ClassId, c.Name!))
                        .ToList();

            SubClassesNames = _context.SubClass
                              .Join(_context.Class,
                                  sub => sub.ClassId,
                                  cls => cls.ClassId,
                                  (sub, cls) => new
                                  {
                                      ClassId = sub.ClassId, 
                                      SubClassName = sub.Name!,
                                      sub.LevelToStart,
                                      sub.SubclassId
                                  })
                              .GroupBy(x => x.ClassId)
                              .ToDictionary(
                                  g => g.Key,
                                  g => g.Select(x => new SubClassInfo
                                  {
                                      SubclassId = x.SubclassId,
                                      SubName = x.SubClassName,
                                      LevelToStart = x.LevelToStart,
                                      ClassId = x.ClassId
                                  }).ToList()
                              );


            RacesNames = _context.Race.Select(r => r.RaceName).ToList();
           Background = _context.Background.Select(b => new BackgroundItem
           {
               BackgroundId = b.BackgroundId,
               Name = b.Name!,
               PrimarySkill = b.PrimarySkill!,
               SecondarySkill = b.SecondarySkill!
           }).ToList();

        }

    }
}