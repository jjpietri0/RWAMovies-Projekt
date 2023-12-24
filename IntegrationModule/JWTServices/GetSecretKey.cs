using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Caching.Memory;

namespace IntegrationModule.JWTServices
{
    public class GetSecretKey
    {

        //private readonly IMemoryCache _cache;
        //private readonly SecretClient _secretClient;
        //private readonly IConfiguration _configuration;


        //public GetSecretKey(IMemoryCache cache, IConfiguration configuration)
        //{
        //    _configuration = configuration;
        //    _cache = cache;
        //    _secretClient = new SecretClient(new Uri("https://rwamovieskey.vault.azure.net/"), new DefaultAzureCredential());
        //}

        //public async Task<string> GetSecretAsync(string secretName, WebApplicationBuilder builder)
        //{
        //    // Try to get the secret from the cache
        //    if (!_cache.TryGetValue(secretName, out string secretValue))
        //    {
        //        // If the secret is not in the cache, retrieve it from Azure Key Vault
        //        KeyVaultSecret secret = await _secretClient.GetSecretAsync(secretName);

        //        // Store the secret in the cache for 24 hours
        //        var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(24));
        //        _cache.Set(secretName, secret.Value, cacheEntryOptions);
        //        secretValue = secret.Value;
        //        _configuration["Jwt:Key"] = secret.Value;
        //    }
        //    return secretValue;
        //}
    }
}
