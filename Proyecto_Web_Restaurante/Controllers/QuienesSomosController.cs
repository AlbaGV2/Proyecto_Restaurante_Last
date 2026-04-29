using Microsoft.AspNetCore.Mvc;

namespace Restaurante.Controllers
{
    public class QuienesSomosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
