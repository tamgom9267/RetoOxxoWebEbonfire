using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RetoOxxoWeb.Model;
using System.Collections.Generic;

namespace RetoOxxoWeb.Pages;

public class JuegoModel : PageModel
{
    public string userFoto { get; set; }

    public List<InfoJuego> DatosJuego;
    private readonly DataBaseContext _context;

    public JuegoModel()
    {
        _context = new DataBaseContext();
    }

    public void OnGet()
    {
        userFoto = HttpContext.Session.GetString("userFoto");
        DatosJuego = _context.GetDatosJuego();
    }
}
