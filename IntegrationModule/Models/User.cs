﻿namespace IntegrationModule.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string PwdHash { get; set; } = null!;
        public string PwdSalt { get; set; } = null!;
        public string? SecurityToken { get; set; }
        public string? Phone { get; set; }
        public bool IsConfirmed { get; set; }
        public virtual Country CountryOfResidence { get; set; } = null!;
        public int CountryOfResidenceId { get; set; }
    }
}
