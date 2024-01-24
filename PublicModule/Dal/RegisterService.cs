using IntegrationModule.REQModels;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PublicModule.Properties;
using System.Text;

namespace PublicModule.Dal
{
    public class RegisterService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseApiUrl;

        public RegisterService(HttpClient httpClient, IOptions<Api> api)
        {
            _httpClient = httpClient;
            _baseApiUrl = api.Value.BaseApiUrl;
        }

        public async Task<HttpResponseMessage> Register(UserRegisterReq userRegisterReq)
        {
            var content = new StringContent(JsonConvert.SerializeObject(userRegisterReq), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_baseApiUrl}/Users/Register", content);
            return response;
        }
    } 
}
