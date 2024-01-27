using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TopBusToursLondon_Model.Models
{
    public class Tour
    {
        public int Id { get; set; }

        [ForeignKey("Category")]
        [Display(Name = "Category Name")]
        public int? CategoryId { get; set; }

        public virtual Category Category { get; set; }

        [Required(ErrorMessage = "Tour Name is required.")]
        [Display(Name = "Tour Name")]
        [Remote("IsAlreadyExists", "Tour", AdditionalFields = "Id,CategoryId", ErrorMessage = "This Tour Name already exists")]
        public string TourName { get; set; }

        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Tour Description is required.")]
        public string Description { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        [DisplayName("Thumbnail")]
        public string ThumbnailImage { get; set; }

        [NotMapped]
        [Display(Name = "Thumbnail Image")]
        public IFormFile ThumbnailImageFile { get; set; }

        [Display(Name = "Cover")]
        public string CoverImage { get; set; }

        [NotMapped]
        [Display(Name = "Cover Image")]
        public IFormFile CoverImageFile { get; set; }

        public ICollection<Ticket> Tickets { get; set; }
        public ICollection<Route> Routes { get; set; }
        public ICollection<Feature> Features { get; set; }
        public ICollection<Highlight> Highlights { get; set; }
    }
}