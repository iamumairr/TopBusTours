using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TopBusToursLondon.ViewModels;
using TopBusToursLondon_Business.Data;
using TopBusToursLondon_Model.Models;

namespace TopBusToursLondon.Controllers
{
    public class FeatureController : Controller
    {
        private readonly ApplicationDbContext _db;

        public FeatureController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Feature> FeaturesList = _db.Features.Include(u => u.Tour).Include(u => u.Ticket);
            return View(FeaturesList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            FeatureViewModel fvm = new FeatureViewModel()
            {
                Feature = new Feature(),
                TourSelectList = _db.Tours.Select(i => new SelectListItem
                {
                    Text = i.TourName,
                    Value = i.Id.ToString()
                })
            };
            return View(fvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(FeatureViewModel fvm)
        {
            if (ModelState.IsValid)
            {
                string[] features = fvm.Feature.Description.Split(',');
                foreach (var item in features)
                {
                    if (item != "")
                    {
                        Feature feature = new Feature();
                        feature.Description = item;
                        feature.TourId = fvm.Feature.TourId;
                        feature.TicketId = fvm.Feature.TicketId;
                        feature.IsActive = fvm.Feature.IsActive;
                        _db.Features.Add(feature);
                        _db.SaveChanges();
                    }
                }
                return RedirectToAction("Index");
            }
            fvm = new FeatureViewModel()
            {
                Feature = new Feature(),
                TourSelectList = _db.Tours.Select(i => new SelectListItem
                {
                    Text = i.TourName,
                    Value = i.Id.ToString()
                })
            };
            return View(fvm);
        }

        public IActionResult Update(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var featureforUpdate = _db.Features.Include(t => t.Ticket).Include(t => t.Tour).FirstOrDefault(t => t.Id == id);
            if (featureforUpdate == null)
            {
                return NotFound();
            }

            FeatureUpdateViewModel fuvm = new FeatureUpdateViewModel()
            {
                Id = featureforUpdate.Id,
                Tour = featureforUpdate.Tour.TourName,
                Ticket = featureforUpdate.Ticket.TicketName,
                Description = featureforUpdate.Description,
                IsActive = featureforUpdate.IsActive
            };

            return View(fuvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(FeatureUpdateViewModel fvm)
        {
            if (ModelState.IsValid)
            {
                var feature = _db.Features.AsNoTracking().FirstOrDefault(c => c.Id == fvm.Id);

                feature.Description = fvm.Description;
                feature.IsActive = fvm.IsActive;

                _db.Features.Update(feature);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(fvm);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var objFromDb = await _db.Features.FindAsync(id);
            if (objFromDb == null)
            {
                return NotFound();
            }
            _db.Features.Remove(objFromDb);
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