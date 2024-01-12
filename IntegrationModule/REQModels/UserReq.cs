namespace IntegrationModule.REQModels
{
    public class UserReq
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsConfirmed { get; set; }
        public int CountryOfResidenceId { get; set; }
    }
}
