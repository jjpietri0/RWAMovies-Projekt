using System.ComponentModel.DataAnnotations;

namespace IntegrationModule.REQModels
{
    public class ResetPasswordReq
    {
        public int Id { get; set; }
        public string Username { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Password must be between 6 and 100 characters long", MinimumLength = 6)]
        public string NewPassword { get; set; }
        [Required]
        public string CurrentPassword { get; set; }


    }
}
