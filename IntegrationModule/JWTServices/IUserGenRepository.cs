using IntegrationModule.Models;
using IntegrationModule.REQModels;

namespace IntegrationModule.Services
{
    public interface IUserGenRepository
    {
        User Add(UserRegisterReq request);
        Tokens JwtTokens(JwtTokensReq request);
    }
}