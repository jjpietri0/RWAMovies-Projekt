using AdminModule.Properties;
using IntegrationModule.REQModels;
using IntegrationModule.ResponseModels;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace AdminModule.Dal
{
    public class VideoService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public VideoService(HttpClient httpClient, IOptions<Api> apiConfig)
        {
            _httpClient = httpClient;
            _baseUrl = apiConfig.Value.BaseUrl;
        }

        public async Task<IEnumerable<VideoResponse>> GetAllVideosAsync(int? page, string? filter)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/Videos/GetAll?page={page}&filter={filter}");
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<VideoResponse>>(content);
        }

        public async Task<IEnumerable<VideoResponse>> GetAllVideosWithGenreFilterAsync(int page, int pageSize, string nameFilter, string genreFilter)
        {
            var url = $"{_baseUrl}/Videos/GetAll?page={page}&pageSize={pageSize}";

            if (!string.IsNullOrEmpty(nameFilter))
            {
                url += $"&name={nameFilter}";
            }

            if (!string.IsNullOrEmpty(genreFilter))
            {
                url += $"&genre={genreFilter}";
            }

            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<VideoResponse>>(content);
        }

        public async Task<VideoResponse> GetVideoByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/Videos/GetById/{id}");
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<VideoResponse>(content);
        }

        public async Task CreateVideoAsync(VideoReq video)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(video), Encoding.UTF8, "application/json");
                await _httpClient.PostAsync($"{_baseUrl}/Videos/Create", content);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateVideoAsync(int id, VideoReq video)
        {
            var content = new StringContent(JsonConvert.SerializeObject(video), Encoding.UTF8, "application/json");
            await _httpClient.PutAsync($"{_baseUrl}/Videos/Update/{id}", content);
        }

        public async Task DeleteVideoAsync(int id)
        {
            await _httpClient.DeleteAsync($"{_baseUrl}/Videos/Delete/{id}");
        }

        public async Task<string> GetJwtTokenForAdmin(string username, string password)
        {
            var loginRequest = new { Username = username, Password = password };
            var content = new StringContent(JsonConvert.SerializeObject(loginRequest), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}/Users/GetJwtTokens", content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Unable to authenticate");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseContent);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.Token);

            return tokenResponse.Token;
        }
        private class TokenResponse
        {
            public string Token { get; set; }
        }
    }
}
