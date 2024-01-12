using IntegrationModule.Models;
using IntegrationModule.REQModels;
using IntegrationModule.ResponseModels;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PublicModule.Properties;
using System.Text;

namespace PublicModule.Dal
{
    public class LoginService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseApiUrl;

        public LoginService(HttpClient httpClient, IOptions<Api> api)
        {
            _httpClient = httpClient;
            _baseApiUrl = api.Value.BaseApiUrl;
        }

        public async Task<UserSignInResponse> Login(UserLoginReq userLoginReq)
        {
            var content = new StringContent(JsonConvert.SerializeObject(userLoginReq), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_baseApiUrl}/Users/Login", content);
            if (response.IsSuccessStatusCode)
            {
                var jsonview = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<UserSignInResponse>(jsonview);
            }
            else
            {
                throw new Exception("Login failed!");
            }
        }
        public async Task<Tokens> GetTokensAsync(JwtTokensReq jwtTokensReq)
        {
            var content = new StringContent(JsonConvert.SerializeObject(jwtTokensReq), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_baseApiUrl}/Users/GetJwtTokens", content);

            if (response.IsSuccessStatusCode)
            {
                var jsonview = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Tokens>(jsonview);
            }
            else
            {
                throw new Exception("GetTokens failed!");
            }
        }
        
    }
}
