using AdminModule.Properties;
using IntegrationModule.REQModels;
using IntegrationModule.ResponseModels;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace AdminModule.Dal
{
    public class TagService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;


        public TagService(HttpClient httpClient, IOptions<Api> options)
        {
            _httpClient = httpClient;
            _baseUrl = options.Value.BaseUrl;
        }

        public async Task<IEnumerable<TagResponse>> GetAllTagsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/Tags/GetAllTags");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<TagResponse>>(content);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        public async Task<TagResponse> GetTagAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/Tags/GetTagById/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TagResponse>(content);
        }


        public async Task<TagResponse> CreateTagAsync(TagRequest tag)
        {
            var response = await _httpClient.PostAsync($"{_baseUrl}/Tags/Create",
                               new StringContent(JsonConvert.SerializeObject(tag), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TagResponse>(content);
        }

        public async Task UpdateTagAsync(int id, TagRequest tag)
        {
            await _httpClient.PutAsync($"{_baseUrl}/Tags/Update/{id}", new StringContent(JsonConvert.SerializeObject(tag), Encoding.UTF8, "application/json"));
        }

        public async Task DeleteTagAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/Tags/DeleteTag/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
