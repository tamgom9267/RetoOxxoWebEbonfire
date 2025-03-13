using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RetoOxxoWeb.Model;

namespace RetoOxxoWeb.Pages;

public class PrivacyModel : PageModel
{
    private readonly ILogger<PrivacyModel> _logger;
    private readonly DataBaseContext _dbContext;

    // Agrega estas propiedades
    public usuario UsuarioEncima { get; set; }
    public usuario UsuarioDebajo { get; set; }

    public PrivacyModel(ILogger<PrivacyModel> logger)
    {
        _logger = logger;
        _dbContext = new DataBaseContext();
    }

    public void OnGet()
    {
        int idUsuarioActual = 4; // Cambia este valor según tus necesidades
        UsuarioEncima = _dbContext.GetUsuarioEncima(idUsuarioActual);
        UsuarioDebajo = _dbContext.GetUsuarioDebajo(idUsuarioActual);
    }
}