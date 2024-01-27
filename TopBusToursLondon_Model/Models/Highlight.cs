using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TopBusToursLondon_Model.Models
{
    public class Highlight
    {
        public int Id { get; set; }

        [ForeignKey("TourId")]
        public virtual Tour Tour { get; set; }

        [Display(Name = "Tour Name")]
        public int? TourId { get; set; }

        [ForeignKey("TicketId")]
        public virtual Ticket Ticket { get; set; }

        [Display(Name = "Ticket Name")]
        public int? TicketId { get; set; }

        [Required(ErrorMessage = "Please Add One Highlight At Least")]
        public string Description { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
    }
}