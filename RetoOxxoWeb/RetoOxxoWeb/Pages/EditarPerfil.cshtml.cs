using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RetoOxxoWeb.Model;

namespace RetoOxxoWeb.Pages.Perfil
{
    public class EditarPerfilModel : PageModel
    {
        [BindProperty]
        public usuario Usuario { get; set; }

        public IActionResult OnGet(int id)
        {
            var db = new DataBaseContext();
            Usuario = db.GetUsuarioPorId(id);

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
                Console.WriteLine("⚠️ Modelo no válido.");
                return Page();
            }

            var db = new DataBaseContext();
            db.ActualizarUsuario(Usuario);

            Console.WriteLine("✅ Datos actualizados con éxito.");

            // Redirigir a página principal después de guardar
            return RedirectToPage("/Index");
        }
    }
}
