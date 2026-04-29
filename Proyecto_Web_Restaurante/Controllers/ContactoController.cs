using Microsoft.AspNetCore.Mvc;

namespace Restaurante.Controllers
{
    public class ContactoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
