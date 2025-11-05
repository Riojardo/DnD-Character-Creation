using DnD_CharacterCreation.Data;
using DnD_CharacterCreation.Models;
using DnD_CharacterCreation.Services;
using DnD_CharacterCreation.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Policy;

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
                UserId = userId
            };

            _context.Characters.Add(character);
            _context.SaveChanges();
            /*
                        var newClasses = new List<(int? classId, int? level, int? subclassId)>
                              {
                                  (1, 5, 1), // First class: ClassId=1, Level=5, SubclassId=2
                                  (2, 3, 4), // Second class: ClassId=2, Level=3, no subclass
                                  //(null, null, null) // Third class: no class selected
                              };

                        foreach (var (classId, level, subclassId) in newClasses)
                        {
                            if (classId.HasValue)
                            {
                                var characterClass = new CharacterClass
                                {
                                    CharacterId = character.CharacterId,
                                    ClassId = classId.Value,
                                    Level = level ?? 1
                                };

                                _context.CharacterClass.Add(characterClass);
                                _context.SaveChanges();

                                if (subclassId.HasValue)
                                {
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
                            }
                        }

                        _context.SaveChanges();*/

            var newClasses = new List<(int? classId, int? level, int? subclassId)>
                     {
                         (model.Classe_First, model.Levelclasss_First, model.Subclasse_First),
                         (model.Classe_Second, model.Levelclasss_Second, model.Subclasse_Second),
                         (model.Classe_Third, model.Levelclasss_Third, model.Subclasse_Third)
                     };

            foreach (var (classId, level, subclassId) in newClasses)
            {
                if (classId.HasValue)
                {
                    var characterClass = new CharacterClass
                    {
                        CharacterId = character.CharacterId,
                        ClassId = classId.Value,
                        Level = level ?? 1
                    };
                    _context.CharacterClass.Add(characterClass);
                    _context.SaveChanges();

                    if (subclassId.HasValue)
                    {
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
                }
            }

            Console.WriteLine($"{model.Levelclasss_First}, {model.Levelclasss_Second}, {model.Levelclasss_Third}");
            return RedirectToAction("UserCharacter", "Home");

        }
        public IActionResult UserCharacter()
        {
            return View();
        }
    }
}


