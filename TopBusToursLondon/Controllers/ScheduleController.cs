using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TopBusToursLondon_Business.Data;
using TopBusToursLondon_Model.Models;

namespace TopBusToursLondon.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public Schedule Schedule { get; set; }

        public ScheduleController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var Schedule = await _db.Schedules.ToListAsync();
            return View(Schedule);
        }

        public IActionResult Upsert(int? id)
        {
            Schedule = new Schedule();
            if (id == null)
            {
                //create
                return View(Schedule);
            }
            //update
            Schedule = _db.Schedules.FirstOrDefault(u => u.Id == id);
            if (Schedule == null)
            {
                return NotFound();
            }
            return View(Schedule);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert()
        {
            if (ModelState.IsValid)
            {
                if (Schedule.Id == 0)
                {
                    //create
                    _db.Schedules.Add(Schedule);
                }
                else
                {
                    _db.Schedules.Update(Schedule);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Schedule);
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var Schedule = await _db.Schedules.FirstOrDefaultAsync(u => u.Id == id);
            if (Schedule == null)
            {
                return NotFound();
            }
            else
            {
                _db.Schedules.Remove(Schedule);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
        }

        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> IsAlreadyExists(string Key, int id)
        {
            if (id > 0)
                return Json(true);

            return Json(!await _db.Schedules.AnyAsync(c => c.Key == Key));
        }
    }
}