using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TopBusToursLondon.ViewModels;
using TopBusToursLondon_Business.Data;
using TopBusToursLondon_Model.Models;

namespace TopBusToursLondon.Controllers
{
    public class TicketController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _WebHostEnvironment;

        public TicketController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _WebHostEnvironment = webHostEnvironment;
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Ticket> TicketList = _db.Tickets.Include(u => u.Tour);

            return View(TicketList);
        }

        public IActionResult Upsert(int? id)
        {
            TicketViewModel tvm = new TicketViewModel()
            {
                Ticket = new Ticket(),
                TourSelectList = _db.Tours.Select(i => new SelectListItem
                {
                    Text = i.TourName,
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
                tvm.Ticket = _db.Tickets.Find(id)!;
                if (tvm.Ticket == null)
                {
                    return NotFound();
                }
                return View(tvm);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(TicketViewModel tvm)
        {
            if (ModelState.IsValid)
            {
                string wwwrootPath = _WebHostEnvironment.WebRootPath;
                string path = @"\Uploads\Ticket\";
                if (tvm.Ticket.Id == 0)
                {
                    string fileNameCover = Path.GetFileNameWithoutExtension(tvm.Ticket.CoverImageFile.FileName);
                    string fileNameTicket = Path.GetFileNameWithoutExtension(tvm.Ticket.TicketImageFile.FileName);

                    string extensionCover = Path.GetExtension(tvm.Ticket.CoverImageFile.FileName);
                    string extensionTicket = Path.GetExtension(tvm.Ticket.TicketImageFile.FileName);

                    tvm.Ticket.TicketImage = fileNameTicket = fileNameTicket + DateTime.Now.ToString("yyyymmssfff") + extensionTicket;
                    tvm.Ticket.CoverImage = fileNameCover = fileNameCover + DateTime.Now.ToString("yyyyymmssfff") + extensionCover;

                    string pathforcover = Path.Combine(wwwrootPath + path + fileNameCover);
                    string pathforticket = Path.Combine(wwwrootPath + path + fileNameTicket);

                    using (var fileStream = new FileStream(pathforcover, FileMode.Create))
                    {
                        tvm.Ticket.CoverImageFile.CopyTo(fileStream);
                    }

                    using (var fileStream = new FileStream(pathforticket, FileMode.Create))
                    {
                        tvm.Ticket.TicketImageFile.CopyTo(fileStream);
                    }
                    _db.Tickets.Add(tvm.Ticket);
                }
                else
                {
                    //update
                    var objFromDb = _db.Tickets.AsNoTracking().FirstOrDefault(c => c.Id == tvm.Ticket.Id);
                    if (tvm.Ticket.TicketImageFile != null)
                    {
                        string fileNameTicket = Path.GetFileNameWithoutExtension(tvm.Ticket.TicketImageFile.FileName);
                        string extensionTicket = Path.GetExtension(tvm.Ticket.TicketImageFile.FileName);
                        tvm.Ticket.TicketImage = fileNameTicket = fileNameTicket + DateTime.Now.ToString("yyyymmssfff") + extensionTicket;
                        string pathforTicket = Path.Combine(wwwrootPath + path + fileNameTicket);
                        var oldTicketImage = Path.Combine(wwwrootPath + path, objFromDb.TicketImage);
                        if (System.IO.File.Exists(oldTicketImage))
                        {
                            System.IO.File.Delete(oldTicketImage);
                        }
                        using (var fileStream = new FileStream(pathforTicket, FileMode.Create))
                        {
                            tvm.Ticket.TicketImageFile.CopyTo(fileStream);
                        }
                    }
                    else
                    {
                        tvm.Ticket.TicketImage = objFromDb.TicketImage;
                    }
                    if (tvm.Ticket.CoverImageFile != null)
                    {
                        string fileNameCover = Path.GetFileNameWithoutExtension(tvm.Ticket.CoverImageFile.FileName);
                        string extensionCover = Path.GetExtension(tvm.Ticket.CoverImageFile.FileName);
                        tvm.Ticket.CoverImage = fileNameCover = fileNameCover + DateTime.Now.ToString("yyyyymmssfff") + extensionCover;
                        string pathforcover = Path.Combine(wwwrootPath + path + fileNameCover);
                        var oldCoverImage = Path.Combine(wwwrootPath + path, objFromDb.CoverImage);
                        if (System.IO.File.Exists(oldCoverImage))
                        {
                            System.IO.File.Delete(oldCoverImage);
                        }
                        using (var fileStream = new FileStream(pathforcover, FileMode.Create))
                        {
                            tvm.Ticket.CoverImageFile.CopyTo(fileStream);
                        }
                    }
                    else
                    {
                        tvm.Ticket.CoverImage = objFromDb.CoverImage;
                    }
                    _db.Tickets.Update(tvm.Ticket);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            tvm.TourSelectList = _db.Tours.Select(i => new SelectListItem
            {
                Text = i.TourName,
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
            var objFromDb = await _db.Tickets.FindAsync(id);
            if (objFromDb == null)
            {
                return NotFound();
            }
            string path = @"\Uploads\Ticket\";
            string upload = _WebHostEnvironment.WebRootPath + path;

            var oldFileCover = Path.Combine(upload, objFromDb.CoverImage);
            if (System.IO.File.Exists(oldFileCover))
            {
                System.IO.File.Delete(oldFileCover);
            }
            var oldFileTicket = Path.Combine(upload, objFromDb.TicketImage);
            if (System.IO.File.Exists(oldFileTicket))
            {
                System.IO.File.Delete(oldFileTicket);
            }

            _db.Tickets.Remove(objFromDb);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> IsAlreadyExists([Bind(Prefix = "Ticket.TourId")] int TourId, [Bind(Prefix = "Ticket.TicketName")] string TicketName, [Bind(Prefix = "Ticket.Id")] int Id)
        {
            if (Id > 0)
                return Json(true);

            return Json(!await _db.Tickets.AnyAsync(c => c.TicketName == TicketName));
        }
    }
}