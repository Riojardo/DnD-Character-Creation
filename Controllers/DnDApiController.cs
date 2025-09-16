using DnD_CharacterCreation.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;


    public class DnDApiController : Controller
    {
        /*public async Task<IActionResult> GetClasses()
        {
            var model = new DnDLists();

            using var client = new HttpClient();
            var response = await client.GetStringAsync("https://www.dnd5eapi.co/api/2014/classes");
            using var jsonDoc = JsonDocument.Parse(response);

            foreach (var classe in jsonDoc.RootElement.GetProperty("results").EnumerateArray())
            {
                model.ClassesNames.Add(classe.GetProperty("name").GetString());
            }
            return View(model); 
        }*/
    }

