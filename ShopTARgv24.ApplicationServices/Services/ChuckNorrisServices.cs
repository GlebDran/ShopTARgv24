using ShopTARgv24.Core.Dto;
using ShopTARgv24.Core.ServiceInterface;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShopTARgv24.ApplicationServices.Services
{
    public class ChuckNorrisServices : IChuckNorrisServices
    {
        private readonly HttpClient _httpClient;

        public ChuckNorrisServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ChuckNorrisJokeDto> GetRandomJoke()
        {
            
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api.chucknorris.io/jokes/random");

            
            request.Headers.Add("User-Agent", "ShopTARgv24-App");

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();

                
                var jokeDto = JsonSerializer.Deserialize<ChuckNorrisJokeDto>(jsonString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return jokeDto;
            }

            
            return null;
        }
    }
}