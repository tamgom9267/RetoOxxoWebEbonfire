using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RetoOxxoWeb.Model;

namespace RetoOxxoWeb.Pages;

public class PrivacyModel : PageModel
{
    private readonly ILogger<PrivacyModel> _logger;
    private readonly DataBaseContext _dbContext;

    public usuario Laura { get; set; }

    public PrivacyModel(ILogger<PrivacyModel> logger)
    {
        _logger = logger;
        _dbContext = new DataBaseContext();
    }

    public void OnGet()
    {
        int idLaura = 4; // ID de Laura
        Laura = _dbContext.GetUsuarioPorId(idLaura);
    }
}
