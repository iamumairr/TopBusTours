using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TopBusToursLondon_Model.Models
{
    public class Route
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

        [Display(Name = "Route Name")]
        public string RouteName { get; set; }

        public string Description { get; set; }

        [Display(Name = "Route Duration")]
        public string Duration { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        public string Image { get; set; }
        [NotMapped]
        [Display(Name = "Route Image")]
        public IFormFile ImageFile { get; set; }
    }
}
