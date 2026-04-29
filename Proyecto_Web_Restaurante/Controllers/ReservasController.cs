using Microsoft.AspNetCore.Mvc;

namespace Restaurante.Controllers
{
    public class ReservasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
