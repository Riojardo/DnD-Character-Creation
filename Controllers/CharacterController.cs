using DnD_CharacterCreation.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Policy;

namespace DnD_CharacterCreation.Controllers
{
    public class CharacterController : Controller
    {
        private readonly DnD_APIServices _dndApiServices;

        public CharacterController(DnD_APIServices dndApiServices)
        {
            _dndApiServices = dndApiServices;
        }

        public async Task<IActionResult> CreateCharacterClass(string className)
        {
            var characterClass = await _dndApiServices.GetClasseAsync(className);
            if (characterClass == null)
                return NotFound();

            return View(characterClass);
        }
    }
}


