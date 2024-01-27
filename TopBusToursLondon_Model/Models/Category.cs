using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TopBusToursLondon_Model.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Category Name is required")]
        [Display(Name = "Category Name"), Column(TypeName = "nvarchar(250)")]
        [Remote("IsAlreadyExists", "Category", AdditionalFields = "Id", ErrorMessage = "This category already exists")]
        public string CategoryName { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        public ICollection<Tour> Tours { get; set; }
    }
}