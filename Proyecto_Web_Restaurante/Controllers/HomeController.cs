using Microsoft.AspNetCore.Mvc;

namespace Restaurante.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
