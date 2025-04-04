using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RetoOxxoWeb.Model;

namespace RetoOxxoWeb.Pages;

public class LeaderboardModel : PageModel
{
    private readonly DataBaseContext _context;
    public List<UsuarioPuntaje> Puntajes {get; set;}
    
    public LeaderboardModel()
    {
        _context = new DataBaseContext();
    }
    public void OnGet()
    {
        Puntajes = _context.GetPuntos();
    }
}
