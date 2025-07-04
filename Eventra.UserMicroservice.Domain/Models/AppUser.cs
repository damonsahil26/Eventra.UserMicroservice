using Eventra.UserMicroservice.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventra.UserMicroservice.Domain.Models
{
    public class AppUser
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public bool IsEmailConfirmed { get; set; }
        public bool IsPhoneVerified { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string? EmailConfirmToken { get; set; }
        public DateTime? EmailConfirmTokenExpiresAt { get; set; }
        public string PasswordHash { get; set; } = string.Empty;
        public UserType UserTypes { get; set; }
    }
}
