using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RetoOxxoWeb.Model;

namespace RetoOxxoWeb.Pages;

public class IndexModel : PageModel
{
	private readonly DataBaseContext _context;
	public string userFoto { get; set; }
	public usuario UsuarioEncima { get; set; }
	public usuario UsuarioDebajo { get; set; }
	public int ProgresoTaberna { get; set; }
	public int ProgresoLaberinto { get; set; }
	public int ProgresoDecision { get; set; }
	public int id;

	// Nuevos indicadores promedio
	public int EfectividadHorarios { get; set; }
	public int FoodService { get; set; }
	public int Planogramas { get; set; }
	public int EjecucionPromociones { get; set; }
	public int ProgramaLealtad { get; set; }
	public int ClasificacionTiendas { get; set; }

	public IndexModel(DataBaseContext context)
	{
		_context = context;
	}

	public void OnGet()
	{
		int? id = HttpContext.Session.GetInt32("usuarioID");

		if (id == null)
		{
			Response.Redirect("IniciarSesion");
			return;
		}

		userFoto = HttpContext.Session.GetString("userFoto");

		// Obtener promedios de indicadores operativos
		var indicadores = _context.GetPromedioIndicadoresOperativos(id.Value);

		EfectividadHorarios = indicadores.EfectividadHorarios;
		FoodService = indicadores.FoodService;
		Planogramas = indicadores.Planogramas;
		EjecucionPromociones = indicadores.EjecucionPromociones;
		ProgramaLealtad = indicadores.ProgramaLealtad;
		ClasificacionTiendas = indicadores.ClasificacionTiendas;

		// Otros datos
		var (taberna, laberinto, decision) = _context.GetProgresoUsuario(id.Value);
		UsuarioEncima = _context.GetUsuarioEncima(id.Value);
		UsuarioDebajo = _context.GetUsuarioDebajo(id.Value);
		ProgresoTaberna = taberna;
		ProgresoLaberinto = laberinto;
		ProgresoDecision = decision;
	}
}

