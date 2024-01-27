using Microsoft.EntityFrameworkCore;
using TopBusToursLondon_Model.Models;

namespace TopBusToursLondon_Business.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Tour> Tours { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Highlight> Highlights { get; set; }
        public DbSet<Newsletters> Newsletters { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<TicketSchedule> TicketSchedules { get; set; }
    }
}