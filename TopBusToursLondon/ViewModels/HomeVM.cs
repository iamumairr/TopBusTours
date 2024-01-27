using TopBusToursLondon_Model.Models;

namespace TopBusToursLondon.ViewModels
{
    public class HomeVM
    {
        public Newsletters Newsletters { get; set; }
        public IEnumerable<Tour> Tour { get; set; }
    }
}