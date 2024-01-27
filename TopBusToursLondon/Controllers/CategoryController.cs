using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TopBusToursLondon_Business.Data;
using TopBusToursLondon_Model.Models;

namespace TopBusToursLondon.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public Category Category { get; set; }

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var Category = await _db.Categories.ToListAsync();
            return View(Category);
        }

        public IActionResult Upsert(int? id)
        {
            Category = new Category();
            if (id == null)
            {
                //create
                return View(Category);
            }
            //update
            Category = _db.Categories.FirstOrDefault(u => u.Id == id);
            if (Category == null)
            {
                return NotFound();
            }
            return View(Category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert()
        {
            if (ModelState.IsValid)
            {
                if (Category.Id == 0)
                {
                    //create
                    _db.Categories.Add(Category);
                }
                else
                {
                    _db.Categories.Update(Category);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Category);
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var categoryFromDb = await _db.Categories.FirstOrDefaultAsync(u => u.Id == id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            else
            {
                _db.Categories.Remove(categoryFromDb);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
        }

        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> IsAlreadyExists(string CategoryName, int id)
        {
            if (id > 0)
                return Json(true);

            return Json(!await _db.Categories.AnyAsync(c => c.CategoryName == CategoryName));
        }
    }
}