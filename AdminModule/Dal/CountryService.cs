using AdminModule.Properties;
using IntegrationModule.ResponseModels;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace AdminModule.Dal
{
    public class CountryService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public CountryService(HttpClient httpClient, IOptions<Api> apiConfig)
        {
            _httpClient = httpClient;
            _baseUrl = apiConfig.Value.BaseUrl;
        }

        // GET: api/Countries
        public async Task<IEnumerable<CountryResponse>> GetAllCountriesAsync(int page)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/Country?page={page}");
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<CountryResponse>>(content);
        }
    }
}
