using IntegrationModule.REQModels;
using IntegrationModule.ResponseModels;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PublicModule.Properties;
using System.Text;

namespace PublicModule.Dal
{
    public class UserService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseApiUrl;

        public UserService(HttpClient httpClient, IOptions<Api> api)
        {
            _httpClient = httpClient;
            _baseApiUrl = api.Value.BaseApiUrl;
        }

        public async Task<UserResponse> GetIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{_baseApiUrl}/UsersList/GetId/{id}");
            var content = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<UserResponse>(content);
            return user;
        }

        public async Task<HttpResponseMessage> ResetPasswordAsync(ResetPasswordReq resetPasswordReq)
        {
            var json = JsonConvert.SerializeObject(resetPasswordReq);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_baseApiUrl}/Users/ResetPassword", data);
            return response;
        }
    }
}
