using Microsoft.AspNetCore.Mvc;

namespace TopBusToursLondon.Controllers
{
    public class RouteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
