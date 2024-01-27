using Microsoft.AspNetCore.Mvc;

namespace TopBusToursLondon.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
