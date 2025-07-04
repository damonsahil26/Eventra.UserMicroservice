using Eventra.UserMicroservice.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventra.UserMicroservice.Application.DTO
{
    public class RegisterUser
    {
        [Required]
        [Length(1, 50, ErrorMessage = "Length should be between 1 to 50 characters")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [Length(1, 50, ErrorMessage = "Length should be between 1 to 50 characters")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Username")]
        [Length(1, 50, ErrorMessage = "Length should be between 1 to 50 characters")]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [EmailAddress(ErrorMessage = "Must be a valid Email address")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Phone Number")]
        [Phone(ErrorMessage ="Must be a valid phone no.")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;
        public UserType UserTypes { get; set; }
    }
}
