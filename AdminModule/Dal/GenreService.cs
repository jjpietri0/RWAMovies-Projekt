using AdminModule.Properties;
using IntegrationModule.REQModels;
using IntegrationModule.ResponseModels;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace AdminModule.Dal
{
    public class GenreService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public GenreService(HttpClient httpClient, IOptions<Api> apiConfig)
        {
            _httpClient = httpClient;
            _baseUrl = apiConfig.Value.BaseUrl;
        }

        public async Task<IEnumerable<GenreResponse>> GetAllGenresAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/Genres");
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<GenreResponse>>(content);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<GenreResponse> GetGenreByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/Genres/GetGenreById/{id}");
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<GenreResponse>(content);
        }


        public async Task CreateGenreAsync(GenreReq genre)
        {
            var content = new StringContent(JsonConvert.SerializeObject(genre), Encoding.UTF8, "application/json");
            await _httpClient.PostAsync($"{_baseUrl}/Genres/Create", content);
        }

        public async Task UpdateGenreAsync(int id, GenreReq genre)
        {
            var content = new StringContent(JsonConvert.SerializeObject(genre), Encoding.UTF8, "application/json");
            await _httpClient.PutAsync($"{_baseUrl}/Genres/Update/{id}", content);
        }

        public async Task DeleteGenreAsync(int id)
        {
            await _httpClient.DeleteAsync($"{_baseUrl}/Genres/Delete/{id}");
        }
    }
}
