using System.Diagnostics;
using System.Text.Json;
using DnD_CharacterCreation.Models;
using DnD_CharacterCreation.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DnD_CharacterCreation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var model = new DnDLists();

            using var client = new HttpClient();
            
            var getClasses = await client.GetStringAsync("https://www.dnd5eapi.co/api/2014/classes");
            using var jsonDocClasses = JsonDocument.Parse(getClasses);

            foreach (var classe in jsonDocClasses.RootElement.GetProperty("results").EnumerateArray())
            {
                model.ClassesNames.Add(classe.GetProperty("name").GetString());
            }

            var getRaces= await client.GetStringAsync("https://www.dnd5eapi.co/api/2014/classes");
            using var jsonDocRace = JsonDocument.Parse(getRaces);

            foreach (var Race in jsonDocRace.RootElement.GetProperty("results").EnumerateArray())
            {
                model.RacesNames.Add(Race.GetProperty("name").GetString());
            }

            var getBackground = await client.GetStringAsync("https://www.dnd5eapi.co/api/2014/races");
            using var jsonDocbackgroud = JsonDocument.Parse(getBackground);

            foreach (var background in jsonDocbackgroud.RootElement.GetProperty("results").EnumerateArray())
            {
                model.BackgroundNames.Add(background.GetProperty("name").GetString());
            }

            return View(model);
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
