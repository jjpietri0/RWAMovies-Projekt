using System.ComponentModel.DataAnnotations;

namespace IntegrationModule.REQModels
{
    public class UserLoginReq
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
