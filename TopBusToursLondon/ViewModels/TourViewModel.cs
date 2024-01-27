using Microsoft.AspNetCore.Mvc.Rendering;
using TopBusToursLondon_Model.Models;

namespace TopBusToursLondon.ViewModels
{
    public class TourViewModel
    {
        public Tour Tour { get; set; }
        public IEnumerable<SelectListItem> CategorySelectList { get; set; }
    }
}
