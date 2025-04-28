using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RetoOxxoWeb.Model;

namespace RetoOxxoWeb.Pages;

public class Progreso_CapacitacionModel : PageModel
{
    public string userFoto { get; set; }

    public void OnGet()
    {
        userFoto = HttpContext.Session.GetString("userFoto");
    }
}
