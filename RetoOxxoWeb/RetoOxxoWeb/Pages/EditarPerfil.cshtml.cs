using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RetoOxxoWeb.Model;

namespace RetoOxxoWeb.Pages.Perfil
{
    public class EditarPerfilModel : PageModel
    {
        // Esta propiedad se enlaza directamente con los datos recibidos del formulario
        [BindProperty]
        public usuario Usuario { get; set; }
        
        // Método que se ejecuta al cargar la página usando un GET cargando los datos del usuario
        public IActionResult OnGet(int id)
        {
            // Se crea una instancia del contexto de base de datos
            var db = new DataBaseContext();
            // Se obtiene el usuario por el ID proporcionado y se asigna a la propiedad Usuario
            Usuario = db.GetUsuarioPorId(id);

            //Si no se encuentra el usuario se regresa un error
            if (Usuario == null)
                return NotFound();

            return Page();
        }

        // Este es el método que se ejecuta al enviar el formulario usando un POST
        public IActionResult OnPost()
        {
            // Estas son unas lineas que use para que al depurar el codigo se pueda ver que se recibieron los datos 
            // ya que batalle un poco haciendo esta parte
            Console.WriteLine("=== POST recibido ===");
            Console.WriteLine($"ID Usuario: {Usuario.id_usuario}");
            Console.WriteLine($"Nombre: {Usuario.nombre}");
            Console.WriteLine($"CP: {Usuario.cp}");

            // Verifica que el modelo enviado cumpla con las validaciones
            if (!ModelState.IsValid)
            {
                Console.WriteLine("Modelo no válido.");
                // Si el modelo no es válido, se regresa a la misma página para mostrar los errores
                return Page();
            }

            // Se crea una instancia del contexto de base de datos
            // y se actualiza el usuario con los datos recibidos del formulario
            var db = new DataBaseContext();
            // Se obtiene el usuario por el ID proporcionado y se asigna a la propiedad Usuario
            db.ActualizarUsuario(Usuario);

            Console.WriteLine("Datos actualizados con éxito.");

            // Redirigir a página principal después de guardar
            return RedirectToPage("/Index");
        }
    }
}
