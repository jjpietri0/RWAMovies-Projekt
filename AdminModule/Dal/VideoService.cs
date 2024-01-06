using AdminModule.Properties;
using IntegrationModule.Models;
using IntegrationModule.REQModels;
using IntegrationModule.ResponseModels;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace AdminModule.Dal
{
    public class VideoService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;


        public VideoService(HttpClient httpClient, IOptions<Api> options)
        {
            _httpClient = httpClient;
            _baseUrl = options.Value.BaseUrl;
        }

        public async Task<IEnumerable<VideoResponse>> GetAllVideoAsync(int page, string filter)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/Videos/GetAll?page={page}&filter={filter}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<VideoResponse>>(content);
        }

        public async Task<VideoResponse> GetVideoAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/Videos/GetById/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<VideoResponse>(content);
        }

        public async Task<IEnumerable<VideoResponse>> GetVideoWithGenreAsync(int page, int pageSize, string name,string genre)
        {
            var videoUrl = $"{_baseUrl}/Videos/GetAll?page={page}&pageSize={pageSize}";
            if (!string.IsNullOrEmpty(name))
            {
                videoUrl += $"&name={name}";
            }
            if (!string.IsNullOrEmpty(genre))
            {
                videoUrl += $"&genre={genre}";
            }
            var response = await _httpClient.GetAsync(videoUrl);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<VideoResponse>>(content);
        }

        public async Task<VideoResponse> CreateVideoAsync(VideoRequest video)
        {
            var response = await _httpClient.PostAsync($"{_baseUrl}/Videos/Create",
                               new StringContent(JsonConvert.SerializeObject(video), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<VideoResponse>(content);
        }

        public async Task UpdateVideoAsync(int id, VideoRequest video)
        {
            await _httpClient.PutAsync($"{_baseUrl}/Videos/Update/{id}",new StringContent(JsonConvert.SerializeObject(video), Encoding.UTF8, "application/json"));
        }

        public async Task DeleteVideoAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/Videos/Delete/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
