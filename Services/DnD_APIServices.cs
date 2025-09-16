using Azure.Core;
using DnD_CharacterCreation.Models;
using System.Diagnostics;
using System.Net.Http;

namespace DnD_CharacterCreation.Services
{
    public class DnD_APIServices
    {
        private readonly HttpClient _httpClient;

        public DnD_APIServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<characterClasse> GetClasseAsync(string classeName)
        {
            var response = await _httpClient.GetFromJsonAsync<characterClasse>($"https://www.dnd5eapi.co/api/classes/{classeName}");
            return response;
        }

/*      var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Get, "https://www.dnd5eapi.co/api/2014/classes/fighter");
        request.Headers.Add("Accept", "application/json");
        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        Console.WriteLine(await response.Content.ReadAsStringAsync());*/


    }
}
