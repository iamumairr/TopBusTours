using System.ComponentModel.DataAnnotations;

namespace TopBusToursLondon_Model.Models
{
    public class TicketSchedule
    {
        public int Id { get; set; }
        public Ticket Ticket { get; set; }

        [Display(Name = "Ticket")]
        public int TicketId { get; set; }

        public Schedule Schedule { get; set; }

        [Display(Name = "Schedule")]
        public int ScheduleId { get; set; }

        [Required(ErrorMessage = "Value is required.")]
        public string Value { get; set; }
    }
}