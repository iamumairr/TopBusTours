using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MimeKit.Text;
using TopBusToursLondon.ViewModels;
using TopBusToursLondon_Business.Data;

namespace TopBusToursLondon.Controllers
{
    public class NewslettersController : Controller
    {
        private readonly ApplicationDbContext _db;

        public NewslettersController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var newsletters = await _db.Newsletters.ToListAsync();
            return View(newsletters);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HomeVM model)
        {
            if (ModelState.IsValid)
            {
                // create email message
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse("umairkhosa398@gmail.com"));
                email.To.Add(MailboxAddress.Parse("umairkhosa398@gmail.com"));
                email.Subject = "Email for Newsletter Subscriptions";
                email.Body = new TextPart(TextFormat.Html)
                {
                    Text = "<p>Hello</p> <strong>Top Bus Tours London.</strong>" +
                           "<br> This email " + @model.Newsletters.Email +
                           " has subscribed for newsletters."
                };
                // send email
                using var smtp = new SmtpClient();
                await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync("umairkhosa398@gmail.com", "umairkhosa");
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
                await _db.Newsletters.AddAsync(model.Newsletters);
                await _db.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Home");
        }

        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> IsAlreadyExists(string Email, int id)
        {
            if (id > 0)
                return Json(true);

            return Json(!await _db.Newsletters.AnyAsync(c => c.Email == Email));
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var newsletter = await _db.Newsletters.FirstOrDefaultAsync(u => u.Id == id);
            if (newsletter == null)
            {
                return NotFound();
            }
            else
            {
                _db.Newsletters.Remove(newsletter);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
        }
    }
}