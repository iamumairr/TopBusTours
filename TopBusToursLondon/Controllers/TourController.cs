using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TopBusToursLondon.ViewModels;
using TopBusToursLondon_Business.Data;
using TopBusToursLondon_Model.Models;

namespace TopBusToursLondon.Controllers
{
    public class TourController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _WebHostEnvironment;

        public TourController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _WebHostEnvironment = webHostEnvironment;
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Tour> TourList = _db.Tours.Include(u => u.Category);

            return View(TourList);
        }

        public IActionResult Upsert(int? id)
        {
            TourViewModel tvm = new TourViewModel()
            {
                Tour = new Tour(),
                CategorySelectList = _db.Categories.Select(i => new SelectListItem
                {
                    Text = i.CategoryName,
                    Value = i.Id.ToString()
                })
            };

            if (id == null)
            {
                //this is for create
                return View(tvm);
            }
            else
            {
                //this is for edit
                tvm.Tour = _db.Tours.Find(id);
                if (tvm.Tour == null)
                {
                    return NotFound();
                }
                return View(tvm);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(TourViewModel tvm)
        {
            if (ModelState.IsValid)
            {
                string wwwrootPath = _WebHostEnvironment.WebRootPath;
                string path = @"\Uploads\Tour\";
                if (tvm.Tour.Id == 0)
                {
                    string fileNameCover = Path.GetFileNameWithoutExtension(tvm.Tour.CoverImageFile.FileName);
                    string fileNameThumbnail = Path.GetFileNameWithoutExtension(tvm.Tour.ThumbnailImageFile.FileName);

                    string extensionCover = Path.GetExtension(tvm.Tour.CoverImageFile.FileName);
                    string extensionThumbnail = Path.GetExtension(tvm.Tour.ThumbnailImageFile.FileName);

                    tvm.Tour.ThumbnailImage = fileNameThumbnail = fileNameThumbnail + DateTime.Now.ToString("yyyymmssfff") + extensionThumbnail;
                    tvm.Tour.CoverImage = fileNameCover = fileNameCover + DateTime.Now.ToString("yyyyymmssfff") + extensionCover;

                    string pathforcover = Path.Combine(wwwrootPath + path + fileNameCover);
                    string pathforthumbnail = Path.Combine(wwwrootPath + path + fileNameThumbnail);

                    using (var fileStream = new FileStream(pathforcover, FileMode.Create))
                    {
                        tvm.Tour.CoverImageFile.CopyTo(fileStream);
                    }

                    using (var fileStream = new FileStream(pathforthumbnail, FileMode.Create))
                    {
                        tvm.Tour.ThumbnailImageFile.CopyTo(fileStream);
                    }
                    _db.Tours.Add(tvm.Tour);
                }
                else
                {
                    //update
                    var objFromDb = _db.Tours.AsNoTracking().FirstOrDefault(c => c.Id == tvm.Tour.Id);
                    if (tvm.Tour.ThumbnailImageFile != null)
                    {
                        string fileNameThumbnail = Path.GetFileNameWithoutExtension(tvm.Tour.ThumbnailImageFile.FileName);
                        string extensionThumbnail = Path.GetExtension(tvm.Tour.ThumbnailImageFile.FileName);
                        tvm.Tour.ThumbnailImage = fileNameThumbnail = fileNameThumbnail + DateTime.Now.ToString("yyyymmssfff") + extensionThumbnail;
                        string pathforthumbnail = Path.Combine(wwwrootPath + path + fileNameThumbnail);
                        var oldThumbnailImage = Path.Combine(wwwrootPath + path, objFromDb.ThumbnailImage);
                        if (System.IO.File.Exists(oldThumbnailImage))
                        {
                            System.IO.File.Delete(oldThumbnailImage);
                        }
                        using (var fileStream = new FileStream(pathforthumbnail, FileMode.Create))
                        {
                            tvm.Tour.ThumbnailImageFile.CopyTo(fileStream);
                        }
                    }
                    else
                    {
                        tvm.Tour.ThumbnailImage = objFromDb.ThumbnailImage;
                    }
                    if (tvm.Tour.CoverImageFile != null)
                    {
                        string fileNameCover = Path.GetFileNameWithoutExtension(tvm.Tour.CoverImageFile.FileName);
                        string extensionCover = Path.GetExtension(tvm.Tour.CoverImageFile.FileName);
                        tvm.Tour.CoverImage = fileNameCover = fileNameCover + DateTime.Now.ToString("yyyyymmssfff") + extensionCover;
                        string pathforcover = Path.Combine(wwwrootPath + path + fileNameCover);
                        var oldCoverImage = Path.Combine(wwwrootPath + path, objFromDb.CoverImage);
                        if (System.IO.File.Exists(oldCoverImage))
                        {
                            System.IO.File.Delete(oldCoverImage);
                        }
                        using (var fileStream = new FileStream(pathforcover, FileMode.Create))
                        {
                            tvm.Tour.CoverImageFile.CopyTo(fileStream);
                        }
                    }
                    else
                    {
                        tvm.Tour.CoverImage = objFromDb.CoverImage;
                    }
                    _db.Tours.Update(tvm.Tour);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            tvm.CategorySelectList = _db.Categories.Select(i => new SelectListItem
            {
                Text = i.CategoryName,
                Value = i.Id.ToString()
            });
            return View(tvm);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var objFromDb = await _db.Tours.FindAsync(id);
            if (objFromDb == null)
            {
                return NotFound();
            }
            string path = @"\Uploads\Tour\";
            string upload = _WebHostEnvironment.WebRootPath + path;

            var oldFileCover = Path.Combine(upload, objFromDb.ThumbnailImage);
            if (System.IO.File.Exists(oldFileCover))
            {
                System.IO.File.Delete(oldFileCover);
            }
            var oldFileThumbnail = Path.Combine(upload, objFromDb.ThumbnailImage);
            if (System.IO.File.Exists(oldFileThumbnail))
            {
                System.IO.File.Delete(oldFileThumbnail);
            }

            _db.Tours.Remove(objFromDb);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> IsAlreadyExists([Bind(Prefix = "Tour.CategoryId")] int CategoryId, [Bind(Prefix = "Tour.Id")] int Id, [Bind(Prefix = "Tour.TourName")] string TourName)
        {
            if (Id > 0)
                return Json(true);

            return Json(!await _db.Tours.AnyAsync(c => c.TourName == TourName));
        }
    }
}