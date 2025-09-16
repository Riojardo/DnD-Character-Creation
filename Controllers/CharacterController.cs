using DnD_CharacterCreation.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DnD_CharacterCreation.Controllers
{
    public class CharacterController : Controller
    {
        private readonly DnD_APIServices _dnd_APIServices;

        public CharacterController(DnD_APIServices dnd_APIServices)
        {
            _dnd_APIServices = dnd_APIServices;
        }

        public async Task<IActionResult> CreateClasse()
        {
            var fighter = await _dnd_APIServices.GetClasseAsync("fighter");
            Console.WriteLine($"Class: {fighter.Name}, Hit Die: {fighter.hit_die}");
            return Content($"Check your terminal: {fighter.Name}");

        }
    }
}
