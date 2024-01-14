using AdminModule.Properties;
using IntegrationModule.REQModels;
using IntegrationModule.ResponseModels;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace AdminModule.Dal
{
    public class UserService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public UserService(HttpClient httpClient, IOptions<Api> apiConfig)
        {
            _httpClient = httpClient;
            _baseUrl = apiConfig.Value.BaseUrl;
        }

        public async Task<IEnumerable<UserResponse>> GetAllUsersAsync(string usernameFilter = null, string firstnameFilter = null, string lastnameFilter = null , string countryFilter = null)
        {
            var queryString = new List<string>();

            if (!string.IsNullOrEmpty(usernameFilter)) queryString.Add($"usernameFilter={usernameFilter}");
            if (!string.IsNullOrEmpty(firstnameFilter)) queryString.Add($"firstnameFilter={firstnameFilter}");
            if (!string.IsNullOrEmpty(lastnameFilter)) queryString.Add($"lastnameFilter={lastnameFilter}");
            if (!string.IsNullOrEmpty(countryFilter)) queryString.Add($"countryFilter={countryFilter}");

            var query = string.Join("&", queryString);

            var response = await _httpClient.GetAsync($"{_baseUrl}/UsersList/GetAllUsers?{query}");
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<UserResponse>>(content);
        }
        public async Task<UserResponse> GetUserByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/UsersList/GetId/{id}");
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<UserResponse>(content);
        }

        public async Task<HttpResponseMessage> CreateUserAsync(UserRegisterReq user)
        {
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync($"{_baseUrl}/Users/Register", content);
        }

        public async Task UpdateUserAsync(int id, UserReq user)
        {
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            await _httpClient.PutAsync($"{_baseUrl}/UsersList/Update/{id}", content);
        }

        public async Task DeleteUserAsync(int id)
        {
            await _httpClient.DeleteAsync($"{_baseUrl}/UsersList/Delete/{id}");
        }
    }
}