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

        public TagService(HttpClient httpClient, IOptions<Api> apiConfig)
        {
            _httpClient = httpClient;
            _baseUrl = apiConfig.Value.BaseUrl;
        }

        public async Task<IEnumerable<TagResponse>> GetAllTagsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/Tags/GetAllTags");
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<TagResponse>>(content);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<TagResponse> GetTagByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/Tags/GetById/{id}");
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TagResponse>(content);
        }

        public async Task CreateTagAsync(TagReq tag)
        {
            var content = new StringContent(JsonConvert.SerializeObject(tag), Encoding.UTF8, "application/json");
            await _httpClient.PostAsync($"{_baseUrl}/Tags/CreateTag", content);
        }

        public async Task UpdateTagAsync(int id, TagReq tag)
        {
            var content = new StringContent(JsonConvert.SerializeObject(tag), Encoding.UTF8, "application/json");
            await _httpClient.PutAsync($"{_baseUrl}/Tags/UpdateTag/{id}", content);
        }

        public async Task DeleteTagAsync(int id)
        {
            await _httpClient.DeleteAsync($"{_baseUrl}/Tags/DeleteTag/{id}");
        }
    }
}
