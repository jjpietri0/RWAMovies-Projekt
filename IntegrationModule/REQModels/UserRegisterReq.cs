using System.ComponentModel.DataAnnotations;

namespace IntegrationModule.REQModels
{
    public class UserRegisterReq
    {
        [Required, StringLength(30, MinimumLength = 3)]
        public string Username { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string Phone { get; set; }
        [Required]
        public int CountryOfResidenceId { get; set; }
    }
}
