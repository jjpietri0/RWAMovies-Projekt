using IntegrationModule.Models;
using IntegrationModule.REQModels;

namespace IntegrationModule.JWTServices
{
    public interface IUserGenRepository
    {
        User Add(UserRegisterReq request);
        Tokens GenerateJwtTokens(JwtTokensReq request);
    }
}