using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RetoOxxoWeb.Model;

namespace RetoOxxoWeb.Pages.Perfil
{
    public class EditarPerfilModel : PageModel
    {
        [BindProperty]
        public usuario Usuario { get; set; }

        public string userFoto { get; set; }

        public IActionResult OnGet(int id)
        {
            var db = new DataBaseContext();
            Usuario = db.GetUsuarioPorId(id);
            userFoto = HttpContext.Session.GetString("userFoto");

            if (Usuario == null)
                return NotFound();

            return Page();
        }

        public IActionResult OnPost()
        {
            Console.WriteLine("=== POST recibido ===");
            Console.WriteLine($"ID Usuario: {Usuario.id_usuario}");
            Console.WriteLine($"Nombre: {Usuario.nombre}");
            Console.WriteLine($"CP: {Usuario.cp}");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("Modelo no válido.");
                return Page();
            }

            var db = new DataBaseContext();

            // Recuperar el valor original de la imagen para conservarlo
            var usuarioExistente = db.GetUsuarioPorId(Usuario.id_usuario);
            if (usuarioExistente == null)
            {
                return NotFound();
            }

            Usuario.fotografia = usuarioExistente.fotografia;

            db.ActualizarUsuario(Usuario);

            Console.WriteLine("Datos actualizados con éxito.");

            return RedirectToPage("/Index");
        }
    }
}
