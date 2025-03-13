using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RetoOxxoWeb.Model;

namespace RetoOxxoWeb.Pages;


public class IndexModel : PageModel
{
    private readonly DataBaseContext _context;
    public usuario UsuarioEncima {get;set;}
    public usuario UsuarioDebajo {get;set;}
    public int ProgresoTaberna { get; set; }
    public int ProgresoLaberinto { get; set; }
    public int ProgresoDecision { get; set; }

    public IndexModel(DataBaseContext context)
    {
        _context = context;
    }

    public void OnGet()
    {
        UsuarioEncima = _context.GetUsuarioEncima(4);
        

    }
}
