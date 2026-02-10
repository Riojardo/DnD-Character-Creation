using DnD_CharacterCreation.Data;
using DnD_CharacterCreation.Models;
using DnD_CharacterCreation.Services;
using DnD_CharacterCreation.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Claims;
using System.Security.Policy;
using System.Text.RegularExpressions;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace DnD_CharacterCreation.Controllers
{
    public class CharacterController : Controller
    {
        private readonly UserManager<Users> _userManager;
        private readonly DnD_APIServices _dndApiServices;
        private readonly AppDbContext _context;

        public CharacterController(DnD_APIServices dndApiServices, AppDbContext context, UserManager<Users> userManager)
        {
            _dndApiServices = dndApiServices;
            _context = context;
            _userManager = userManager; 
        }
        public async Task<IActionResult> CreateCharacterClass(string className)
        {
            var characterClass = await _dndApiServices.GetClasseAsync(className);
            if (characterClass == null)
                return NotFound();

            return View(characterClass);
        }
        [HttpGet]
        public IActionResult Create()
        {

            var model = new CharacterViewModel
            {
                CharacterName = string.Empty,
                Alignment = string.Empty,
                BackgroundId = 0,
                Species = 0,
                Subspecies = 0,
                total_STR = "2",
                total_DEX = "3",
                total_CON = "4",
                total_INT = "5",
                total_WIS = "6",
                total_CHA = "7",
            };

            return View(model);
        }
        [HttpPost]
        public IActionResult Create(CharacterViewModel model)
        {
            var userId = _userManager.GetUserId(User);

            var character = new Character
            {
                CharacterName = model.CharacterName,
                Alignment = model.Alignment,
                BackgroundId = model.BackgroundId,
                RaceId = model.Species,
                SubRaceId = model.Subspecies,
                UserId = userId,

            };


            _context.Characters.Add(character);
            _context.SaveChanges();
          
            var newClasses = new List<(int? classId, int? level, int? subclassId)>
                     {
                         (model.Classe_First, model.Levelclasss_First, model.Subclasse_First),
                         (model.Classe_Second, model.Levelclasss_Second, model.Subclasse_Second),
                         (model.Classe_Third, model.Levelclasss_Third, model.Subclasse_Third)
                     };

            foreach (var (classId, level, subclassId) in newClasses)
            {
                if (!classId.HasValue)
                { continue; }

                var characterClass = new CharacterClass
                    {
                        CharacterId = character.CharacterId,
                        ClassId = classId.Value,
                        Level = level ?? 1
                    };
                    _context.CharacterClass.Add(characterClass);
                    _context.SaveChanges();

                    if (!subclassId.HasValue)
                    { continue; }
                    var sub = new CharacterSubClass
                        {
                            CharacterId = character.CharacterId,
                            ClassId = classId.Value,
                            SubclassId = subclassId.Value,
                            CharacterClassId = characterClass.CharacterClassId
                        };
                        _context.CharacterSubClass.Add(sub);
                        _context.SaveChanges();
                                
            }

            var characterStats = new CharacterAbility
            {
                CharacterId = character.CharacterId,
                Strength = string.IsNullOrEmpty(model.total_STR) ? 0 : Convert.ToInt32(model.total_STR),
                Dexterity = string.IsNullOrEmpty(model.total_DEX) ? 0 : Convert.ToInt32(model.total_DEX),
                Constitution = string.IsNullOrEmpty(model.total_CON) ? 0 : Convert.ToInt32(model.total_CON),
                Intelligence = string.IsNullOrEmpty(model.total_INT) ? 0 : Convert.ToInt32(model.total_INT),
                Wisdom = string.IsNullOrEmpty(model.total_WIS) ? 0 : Convert.ToInt32(model.total_WIS),
                Charisma = string.IsNullOrEmpty(model.total_CHA) ? 0 : Convert.ToInt32(model.total_CHA),
   
            };
            _context.CharacterAbility.Add(characterStats);
            _context.SaveChanges();

            var characterSkills = new CharacterSkill
            {
                CharacterId = character.CharacterId,
                Acrobatics = model.Acrobatics,
                AnimalHandling = model.AnimalHandling,
                Arcana = model.Arcana,
                Athletics = model.Athletics,
                Deception = model.Deception,
                History = model.History,
                Insight = model.Insight,
                Intimidation = model.Intimidation,
                Investigation = model.Investigation,
                Medicine = model.Medicine,
                Nature = model.Nature,
                Perception = model.Perception,
                Performance = model.Performance,
                Persuasion = model.Persuasion,
                Religion = model.Religion,
                SleightOfHand = model.SleightOfHand,
                Stealth = model.Stealth,
                Survival = model.Survival
            };
            _context.CharacterSkill.Add(characterSkills);
            _context.SaveChanges();

            return RedirectToAction("UserCharacter", "Home");

        }
        public IActionResult UserCharacter()
        {
            return View();
        }
    }
}


