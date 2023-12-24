namespace IntegrationModule.ResponseModels
{
    public partial class UserResponse
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int CountryOfResidenceId { get; set; }
        public string? Phone { get; set; }
        public bool IsConfirmed { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
