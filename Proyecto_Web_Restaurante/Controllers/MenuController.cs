using Microsoft.AspNetCore.Mvc;

namespace Restaurante.Controllers
{
    public class MenuController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
