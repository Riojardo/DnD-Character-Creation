using DnD_CharacterCreation.Data;
using DnD_CharacterCreation.Models;
using DnD_CharacterCreation.Services;
using DnD_CharacterCreation.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

            return RedirectToAction("UserCharacter", "Home");
        }
        public IActionResult UserCharacter()
        {
            return View();
        }
    }
}


