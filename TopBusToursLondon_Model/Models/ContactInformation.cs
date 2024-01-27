using System.ComponentModel.DataAnnotations;

namespace TopBusToursLondon_Model.Models
{
    public class ContactInformation
    {
        public int Id { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Enter your name")]
        [MaxLength(50, ErrorMessage = "Name can't be more 50 characters.")]
        public string FromName { get; set; }

        [Required(ErrorMessage = "Email is required."), Display(Name = "Email Address"), EmailAddress]
        [MaxLength(255, ErrorMessage = "Email address can't be larger than 255 characters")]
        public string FromEmail { get; set; }

        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Message Field is required.")]
        public string Message { get; set; }
    }
}
