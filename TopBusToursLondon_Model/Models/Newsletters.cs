using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TopBusToursLondon_Model.Models
{
    public class Newsletters
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Email Address is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address (Ex:john@domain.com)")]
        [MaxLength(255, ErrorMessage = "Email address can't be more than 255 characters.")]
        [Remote("IsAlreadyExists", "Newsletters", AdditionalFields = "Id", ErrorMessage = "This Email already exists")]
        public string Email { get; set; }

        public bool IsActive { get; set; }
        public bool IsVerified { get; set; }
    }
}