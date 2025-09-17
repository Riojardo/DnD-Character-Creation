using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DnD_CharacterCreation.Models;

namespace DnD_CharacterCreation.Services
{
    public class DnD_APIServices
    {
        private readonly HttpClient _httpClient;

        public DnD_APIServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Fetch a class from the D&D API by its name.
        /// </summary>
        /// <param name="classeName">The class name, e.g., "fighter"</param>
        /// <returns>A CharacterClass object populated with API data</returns>
        public async Task<CharacterClass> GetClasseAsync(string classeName)
        {
            // Make the HTTP GET request to the API
            var response = await _httpClient.GetFromJsonAsync<ApiClassResponse>($"https://www.dnd5eapi.co/api/classes/{classeName}");

            if (response == null)
                return null;

            // Map API response to your CharacterClass model
            var characterClass = new CharacterClass
            {
                ClassId = 0,
                
            };

            return characterClass;
        }
    }

    // Minimal class to match D&D API JSON
    public class ApiClassResponse
    {
        public string index { get; set; }
        public string name { get; set; }
        public int hit_die { get; set; }
        // Add more fields if needed
    }
}