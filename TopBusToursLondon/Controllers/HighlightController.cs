using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TopBusToursLondon.ViewModels;
using TopBusToursLondon_Business.Data;
using TopBusToursLondon_Model.Models;

namespace TopBusToursLondon.Controllers
{
    public class HighlightController : Controller
    {
        private readonly ApplicationDbContext _db;

        public HighlightController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Highlight> HighlightList = _db.Highlights.Include(u => u.Tour).Include(u => u.Ticket);
            return View(HighlightList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            HighlightViewModel hvm = new HighlightViewModel()
            {
                Highlight = new Highlight(),
                TourSelectList = _db.Tours.Select(i => new SelectListItem
                {
                    Text = i.TourName,
                    Value = i.Id.ToString()
                })
            };

            return View(hvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(HighlightViewModel hvm)
        {
            if (ModelState.IsValid)
            {
                string[] highlights = hvm.Highlight.Description.Split(',');
                foreach (var item in highlights)
                {
                    if (item != "")
                    {
                        Highlight highlight = new Highlight();
                        highlight.Description = item;
                        highlight.TourId = hvm.Highlight.TourId;
                        highlight.TicketId = hvm.Highlight.TicketId;
                        highlight.IsActive = hvm.Highlight.IsActive;
                        _db.Highlights.Add(highlight);
                        _db.SaveChanges();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            hvm = new HighlightViewModel()
            {
                Highlight = new Highlight(),
                TourSelectList = _db.Tours.Select(i => new SelectListItem
                {
                    Text = i.TourName,
                    Value = i.Id.ToString()
                })
            };
            return View(hvm);
        }

        public IActionResult Update(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }
            var highlightforUpdate = _db.Highlights.Include(u => u.Ticket).Include(u => u.Tour).FirstOrDefault(t => t.Id == id);
            if (highlightforUpdate == null)
            {
                return NotFound();
            }

            HighlightUpdateViewModel huvm = new HighlightUpdateViewModel()
            {
                Id = highlightforUpdate.Id,
                Tour = highlightforUpdate.Tour.TourName,
                Ticket = highlightforUpdate.Ticket.TicketName,
                Description = highlightforUpdate.Description,
                IsActive = highlightforUpdate.IsActive
            };
            return View(huvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(FeatureUpdateViewModel hvm)
        {
            if (ModelState.IsValid)
            {
                var highlight = _db.Highlights.AsNoTracking().FirstOrDefault(c => c.Id == hvm.Id);

                highlight.Description = hvm.Description;
                highlight.IsActive = hvm.IsActive;

                _db.Highlights.Update(highlight);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hvm);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var objFromDb = await _db.Highlights.FindAsync(id);
            if (objFromDb == null)
            {
                return NotFound();
            }
            _db.Highlights.Remove(objFromDb);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult GetTicketList(int TourId)
        {
            var TicketList = new SelectList(_db.Tickets.Where(c => c.Tour.Id == TourId), "Id", "TicketName");
            return Json(TicketList);
        }
    }
}