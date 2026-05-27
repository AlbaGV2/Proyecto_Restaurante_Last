using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Restaurante.Controllers
{
    public class AuthController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            // Si ya está autenticado, redirigir al gestor
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Gestion", "Reservas");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password))
            {
                return Json(new { success = false, message = "Por favor introduce tu usuario y contraseña." });
            }

            string userLower = model.Username.Trim().ToLower();
            string pass = model.Password.Trim();

            bool isValid = false;
            if ((userLower == "mikel@restaurante.es" && pass == "Mikel123") ||
                (userLower == "alba@restaurante.es" && pass == "Alba123"))
            {
                isValid = true;
            }

            if (isValid)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userLower),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = System.DateTimeOffset.UtcNow.AddHours(2)
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                return Json(new { success = true, redirectUrl = "/Reservas/Gestion" });
            }

            return Json(new { success = false, message = "Usuario o contraseña incorrectos." });
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Registro()
        {
            return View();
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
