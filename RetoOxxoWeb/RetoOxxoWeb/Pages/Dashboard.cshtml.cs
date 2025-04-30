using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;

namespace RetoOxxoWeb.Pages
{
    public class DashboardModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int? param_usuario { get; set; }

        public string userFoto { get; set; }

        public void OnGet()
        {
            if (param_usuario == null)
            {
                // Si no hay param_usuario, redirige al login
                Response.Redirect("/IniciarSesion");
            }

            userFoto = HttpContext.Session.GetString("userFoto") 
                ?? "https://cdn.pixabay.com/photo/2021/06/07/13/46/user-6318011_1280.png";
        }
    }
}
