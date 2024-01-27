using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TopBusToursLondon.ViewModels;
using TopBusToursLondon_Business.Data;
using TopBusToursLondon_Model.Models;

namespace TopBusToursLondon.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment, ILogger<HomeController> logger)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            HomeVM hvm = new HomeVM()
            {
                Tour = _db.Tours
            };
            return View(hvm);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactInformation model)
        {
            return View(model);
        }

        public IActionResult BigBus()
        {
            return View();
        }

        public IActionResult GoldenTours()
        {
            return View();
        }

        public IActionResult Original()
        {
            return View();
        }

        public IActionResult Stonehenge()
        {
            return View();
        }

        public IActionResult Attractions()
        {
            return View();
        }

        public IActionResult HarryPotter()
        {
            return View();
        }

        public IActionResult ShowTickets(int id)
        {
            var tour = _db.Tours.Include(u => u.Tickets).Include(u => u.Features).Include(u => u.Highlights).Where(u => u.Id == id).ToList();
            TicketVM tvm = new TicketVM();
            foreach (var item in tour)
            {
                tvm.TourName = item.TourName;
                tvm.CoverImage = item.CoverImage;
                tvm.Tickets = _db.Tickets.Where(e => e.TourId == item.Id).ToList();
            }
            return View(tvm);
        }

        public IActionResult TicketDetails(int id)
        {
            var Ticket = _db.Tickets.Where(a => a.Id == id).Include(b => b.Highlights).Include(c => c.TicketSchedules).ThenInclude(d => d.Schedule).SingleOrDefault();
            return View(Ticket);
        }

        public IActionResult Services()
        {
            return View();
        }

        public IActionResult FAQ()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Terms()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}