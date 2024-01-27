using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TopBusToursLondon.ViewModels;
using TopBusToursLondon_Business.Data;
using TopBusToursLondon_Model.Models;

namespace TopBusToursLondon.Controllers
{
    public class TicketScheduleController : Controller
    {
        private readonly ApplicationDbContext _db;

        public TicketScheduleController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<TicketSchedule> TicketSchedule = await _db.TicketSchedules.Include(u => u.Ticket).Include(u => u.Schedule).ToListAsync();
            return View(TicketSchedule);
        }

        public IActionResult Create()
        {
            TicketScheduleVM tsvm = new TicketScheduleVM()
            {
                TicketSchedule = new TicketSchedule(),
                TicketSelectList = _db.Tickets.Select(i => new SelectListItem
                {
                    Text = i.Tour.TourName + " - " + i.TicketName,
                    Value = i.Id.ToString()
                }),
                ScheduleSelectList = _db.Schedules.Select(i => new SelectListItem
                {
                    Text = i.Key,
                    Value = i.Id.ToString()
                })
            };
            return View(tsvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TicketScheduleVM tsvm)
        {
            if (ModelState.IsValid)
            {
                _db.TicketSchedules.Add(tsvm.TicketSchedule);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            tsvm.TicketSelectList = _db.Tickets.Select(i => new SelectListItem
            {
                Text = i.Tour.TourName + " - " + i.TicketName,
                Value = i.Id.ToString()
            });
            tsvm.ScheduleSelectList = _db.Schedules.Select(i => new SelectListItem
            {
                Text = i.Key,
                Value = i.Id.ToString()
            });
            return View(tsvm);
        }

        public IActionResult Update(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }
            TicketScheduleVM tsvm = new TicketScheduleVM();
            tsvm.TicketSchedule = _db.TicketSchedules.Include(t => t.Ticket).Include(t => t.Schedule).FirstOrDefault(t => t.Id == id);
            if (tsvm.TicketSchedule == null)
            {
                return NotFound();
            }
            tsvm.TicketSelectList = _db.Tickets.Select(i => new SelectListItem
            {
                Text = i.Tour.TourName + " - " + i.TicketName,
                Value = i.Id.ToString()
            });
            tsvm.ScheduleSelectList = _db.Schedules.Select(i => new SelectListItem
            {
                Text = i.Key,
                Value = i.Id.ToString()
            });

            return View(tsvm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Update(TicketScheduleVM tsvm)
        {
            if (ModelState.IsValid)
            {
                _db.TicketSchedules.Update(tsvm.TicketSchedule);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            tsvm.TicketSelectList = _db.Tickets.Select(i => new SelectListItem
            {
                Text = i.Tour.TourName + " - " + i.TicketName,
                Value = i.Id.ToString()
            });
            tsvm.ScheduleSelectList = _db.Schedules.Select(i => new SelectListItem
            {
                Text = i.Key,
                Value = i.Id.ToString()
            });
            return View();
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var ticketschedule = await _db.TicketSchedules.FirstOrDefaultAsync(u => u.Id == id);
            if (ticketschedule == null)
            {
                return NotFound();
            }
            else
            {
                _db.TicketSchedules.Remove(ticketschedule);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
        }
    }
}