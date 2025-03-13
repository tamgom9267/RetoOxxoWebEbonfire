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
    public int FoodService { get; set; }
    public int EjecucionPromociones { get; set; }
    public int EquiposCompletos { get; set; }
    public int Rotacion { get; set; }
    public int FaltanteEfectivo { get; set; }

    public IndexModel(DataBaseContext context)
    {
        _context = context;
    }

    public void OnGet()
    {
        var metricas = _context.GetMetricasDeTienda(4);
        var (taberna, laberinto, decision) = _context.GetProgresoUsuario(4);
        UsuarioEncima = _context.GetUsuarioEncima(4);
        UsuarioDebajo = _context.GetUsuarioDebajo(4);
        ProgresoTaberna = taberna;
        ProgresoLaberinto = laberinto;
        ProgresoDecision = decision;
        FoodService = metricas.FoodService;
        EjecucionPromociones = metricas.EjecucionPromociones;
        EquiposCompletos = metricas.EquiposCompletos;
        Rotacion = metricas.Rotacion;
        FaltanteEfectivo = metricas.FaltanteEfectivo;

    }
}
