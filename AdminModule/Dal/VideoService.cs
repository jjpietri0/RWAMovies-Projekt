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
        private string _jwtToken;

        // Inject the HttpClient and the API configuration.
        public VideoService(HttpClient httpClient, IOptions<Api> apiConfig)
        {
            _httpClient = httpClient;
            _baseUrl = apiConfig.Value.BaseUrl;
        }

        // Get Genre by Id
        public async Task<GenreResponse> GetGenreByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/Genres/GetById/{id}");
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<GenreResponse>(content);
        }

        // Set the JWT token to be used in the Authorization header.
        public void SetJwtToken(string token)
        {
            _jwtToken = token;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwtToken);
        }

        public async Task<IEnumerable<VideoResponse>> GetAllVideosAsync(int page, string filter)
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
            var content = new StringContent(JsonConvert.SerializeObject(video), Encoding.UTF8, "application/json");
            await _httpClient.PostAsync($"{_baseUrl}/Videos/Create", content);
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

        // Obtain a JWT token for the administrative user
        public async Task<string> ObtainJwtTokenForAdmin(string username, string password)
        {
            var loginRequest = new { Username = username, Password = password };
            var content = new StringContent(JsonConvert.SerializeObject(loginRequest), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}/Users/JwtTokens", content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Unable to authenticate");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseContent);

            // Set the token in Authorization header
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.Token);

            return tokenResponse.Token;
        }

        // Add a class for deserializing token response
        private class TokenResponse
        {
            public string Token { get; set; }
        }
    }
}
