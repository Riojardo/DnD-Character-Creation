using System.Diagnostics;
using System.Text.Json;
using DnD_CharacterCreation.Data;
using DnD_CharacterCreation.Models;
using DnD_CharacterCreation.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DnD_CharacterCreation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        private readonly UserManager<Users> _userManager;

        public HomeController(ILogger<HomeController> logger, AppDbContext context, UserManager<Users> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var model = new CharacterViewModel();
            //model.Backgrounds = _context.Background.ToList();

            using var client = new HttpClient();
            model.ListsAPI = new DnDLists(_context);


            /*      var getClasses = await client.GetStringAsync("https://www.dnd5eapi.co/api/2014/classes");
                  using var jsonDocClasses = JsonDocument.Parse(getClasses);
                  foreach (var classe in jsonDocClasses.RootElement.GetProperty("results").EnumerateArray())
                  {
                      model.ListsAPI.ClassesNames.Add(classe.GetProperty("name").GetString());
                  }


                  var getRaces = await client.GetStringAsync("https://www.dnd5eapi.co/api/2014/races");
                  using var jsonDocRaces = JsonDocument.Parse(getRaces);
                  foreach (var race in jsonDocRaces.RootElement.GetProperty("results").EnumerateArray())
                  {
                      model.ListsAPI.RacesNames.Add(race.GetProperty("name").GetString());
                  }
      */
            return View(model);
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize]
     
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult UserCharacter()
        {
            var userId = _userManager.GetUserId(User);

            // On charge tous les personnages pour cet utilisateur, sans tri
            var characters = _context.Characters
                .Where(c => c.UserId == userId)
                .ToList();

            // On passe le modèle à la vue
            return View(characters);
        }
    }
}
