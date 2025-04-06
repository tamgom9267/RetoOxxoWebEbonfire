using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RetoOxxoWeb.Model;

namespace RetoOxxoWeb.Pages;

public class PrivacyModel : PageModel
{
    private readonly ILogger<PrivacyModel> _logger;
    private readonly DataBaseContext _dbContext;

    public usuario user { get; set; }

    public PrivacyModel(ILogger<PrivacyModel> logger)
    {
        _logger = logger;
        _dbContext = new DataBaseContext();
    }

    public void OnGet()
    {
        int? user_id = HttpContext.Session.GetInt32("usuarioID");

        user = _dbContext.GetUsuarioPorId(user_id.Value);
    }
}
