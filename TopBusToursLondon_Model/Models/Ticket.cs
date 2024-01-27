using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TopBusToursLondon_Model.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        [ForeignKey("TourId")]
        public virtual Tour Tour { get; set; }

        [Display(Name = "Tour Name")]
        public int? TourId { get; set; }

        [Display(Name = "Ticket Name")]
        [Required(ErrorMessage = "Ticket Name is required.")]
        [Remote("IsAlreadyExists", "Ticket", AdditionalFields = "Id,TourId", ErrorMessage = "This ticket already exists")]
        public string TicketName { get; set; }

        [Display(Name = "Child Price")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ChildPrice { get; set; }

        [Display(Name = "Adult Price")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal AdultPrice { get; set; }

        [Display(Name = "Family Price")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal FamilyPrice { get; set; }

        [Display(Name = "Is On Offer")]
        public bool IsOnOffer { get; set; }

        [Display(Name = "Child Offer Price")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal OfferChildPrice { get; set; }

        [Display(Name = "Adult Offer Price")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal OfferAdultPrice { get; set; }

        [Display(Name = "Family Offer Price")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal OfferFamilyPrice { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        public string TicketImage { get; set; }

        [NotMapped]
        [Display(Name = "Ticket Image")]
        public IFormFile TicketImageFile { get; set; }

        public string CoverImage { get; set; }

        [NotMapped]
        [Display(Name = "Cover Image")]
        public IFormFile CoverImageFile { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Meeting Point")]
        public string MeetingPoint { get; set; }

        public ICollection<Route> Routes { get; set; }
        public ICollection<Feature> Features { get; set; }
        public ICollection<Highlight> Highlights { get; set; }
        public ICollection<TicketSchedule> TicketSchedules { get; set; }
    }
}