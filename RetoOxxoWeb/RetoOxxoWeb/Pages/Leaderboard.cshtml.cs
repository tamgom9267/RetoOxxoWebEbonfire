using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RetoOxxoWeb.Model;

namespace RetoOxxoWeb.Pages;

public class LeaderboardModel : PageModel
{
    private readonly DataBaseContext _context;
    public List<UsuarioPuntaje> Puntajes {get; set;}
    public List<UsuarioLogros> Logros {get; set;}
    public string PremioDelMes { get; set; } = "Tarjeta de regalo OXXO de $500";  // Puedes cambiarlo dinámicamente
    public string PrimerLugar { get; set; }
    public string userFoto { get; set; }
    public LeaderboardModel()
    {
        _context = new DataBaseContext();
    }
    public void OnGet()
    {
        Puntajes = _context.GetPuntos();
        Logros = _context.GetLogros();
        userFoto = HttpContext.Session.GetString("userFoto");

        if (Puntajes.Any())
        {
            PrimerLugar = Puntajes.First().nombre;  // Obtener el primer lugar
        }
        else
        {
            PrimerLugar = "Nadie aún";
        }
    }
}
