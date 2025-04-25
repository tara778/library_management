using Microsoft.AspNetCore.Mvc;

namespace library_management.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}