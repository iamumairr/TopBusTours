using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TopBusToursLondon_Model.Models
{
    public class Schedule
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Key is required.")]
        [Remote("IsAlreadyExists", "Schedule", AdditionalFields = "Id", ErrorMessage = "This key already exists")]
        public string Key { get; set; }

        public ICollection<TicketSchedule> TicketSchedules { get; set; }
    }
}