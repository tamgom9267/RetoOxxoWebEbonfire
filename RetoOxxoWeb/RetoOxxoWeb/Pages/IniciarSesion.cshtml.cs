using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RetoOxxoWeb.Model;
using System.Collections.Generic;
using System.Diagnostics;

namespace RetoOxxoWeb.Pages
{
    public class LogInModel : PageModel
    {
        [BindProperty]
        public string Nombre { get; set; }
        
        [BindProperty]
        public string Contraseña { get; set; }

        public string Mensaje { get; set; }

        public IActionResult OnPost()
        {
            DataBaseContext db = new DataBaseContext();
            List<usuario> usuarios = db.GetAllUsers(); // Obtener todos los usuarios de la base de datos

            Debug.WriteLine($"Usuario ingresado: {Nombre}, Contraseña ingresada: {Contraseña}");

            // Verificar si el usuario y la contraseña coinciden
            var usuarioValido = usuarios.Find(u => u.nombre.Trim() == Nombre.Trim() && u.contraseña.Trim() == Contraseña.Trim());

            if (usuarioValido != null)
            {
                Debug.WriteLine("Inicio de sesión exitoso. Redirigiendo...");
                Response.Redirect("/Index");
                return Page(); // Asegura que el método no continúe ejecutándose
            }
            else
            {
                Debug.WriteLine("Credenciales incorrectas.");
                Mensaje = "Credenciales incorrectas. Inténtalo de nuevo.";
                return Page();
            }
        }
    }
}
