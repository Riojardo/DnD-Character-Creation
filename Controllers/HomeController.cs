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

            var characters = _context.Characters
                .Where(c => c.UserId == userId)
                .ToList();


            return View(characters);
        }
    }
}
