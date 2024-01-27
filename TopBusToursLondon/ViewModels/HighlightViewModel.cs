using Microsoft.AspNetCore.Mvc.Rendering;
using TopBusToursLondon_Model.Models;

namespace TopBusToursLondon.ViewModels
{
    public class HighlightViewModel
    {
        public Highlight Highlight { get; set; }
        public IEnumerable<SelectListItem> TourSelectList { get; set; }
    }

    public class HighlightUpdateViewModel
    {
        public int Id { get; set; }
        public string Tour { get; set; }
        public string Ticket { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}