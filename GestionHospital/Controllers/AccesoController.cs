using GestionHospital.Data;
using GestionHospital.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace GestionHospital.Controllers
{
    public class AccesoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Usuario usuario)
        {
            LogicaUsuario logicaUsuario = new LogicaUsuario();

            var user = logicaUsuario.ValidarUsuario(usuario.Email, usuario.Pass);

            if(user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim("Email", user.Email),
                    new Claim("Pass", user.Pass)
                };

                foreach (string rol in user.Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, rol));
                }

                var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimIdentity));

                return RedirectToAction("Index", "Home");
            }else
                return View();
        }

        public async Task<IActionResult> Salir()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Acceso"); ;
        }
    }
}
