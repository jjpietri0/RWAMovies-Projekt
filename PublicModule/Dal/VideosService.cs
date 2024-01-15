using IntegrationModule.ResponseModels;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PublicModule.Properties;
using System.Net.Http.Headers;

namespace PublicModule.Dal
{
    public class VideosService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseApiUrl;

        public VideosService(HttpClient httpClient, IOptions<Api> api)
        {
            _httpClient = httpClient;
            _baseApiUrl = api.Value.BaseApiUrl;
        }

        public async Task<IEnumerable<VideoResponse>> GetVideosAsync(string videoName, string genre, string orderBy, int page, int pageSize, string accessToken)
        {
            SetAuthorizationHeaders(accessToken);
            var response = await _httpClient.GetAsync($"{_baseApiUrl}/Videos/GetAll?name={videoName}&genre={genre}&orderBy={orderBy}&page={page}&pageSize={pageSize}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var videos = JsonConvert.DeserializeObject<IEnumerable<VideoResponse>>(content);
            return videos;
        }

        private void SetAuthorizationHeaders(string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            var authHeader = new AuthenticationHeaderValue("Bearer", accessToken);
            _httpClient.DefaultRequestHeaders.Authorization = authHeader;
        }

        public async Task<VideoResponse> GetVideoIdAsync(int id, string accessToken)
        {
            SetAuthorizationHeaders(accessToken);
            var response = await _httpClient.GetAsync($"{_baseApiUrl}/Videos/GetId/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var video = JsonConvert.DeserializeObject<VideoResponse>(content);
            return video;
        }

    }
}
