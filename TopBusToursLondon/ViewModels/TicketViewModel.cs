using Microsoft.AspNetCore.Mvc.Rendering;
using TopBusToursLondon_Model.Models;

namespace TopBusToursLondon.ViewModels
{
    public class TicketViewModel
    {
        public Ticket Ticket { get; set; }
        public IEnumerable<SelectListItem> TourSelectList { get; set; }
    }

    public class TicketVM
    {
        public string CoverImage { get; set; }
        public string TourName { get; set; }
        public IEnumerable<Ticket> Tickets { get; set; }
    }

    public class TicketScheduleVM
    {
        public TicketSchedule TicketSchedule { get; set; }
        public IEnumerable<SelectListItem> TicketSelectList { get; set; }
        public IEnumerable<SelectListItem> ScheduleSelectList { get; set; }
    }
}