using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RetoOxxoWeb.Model;
using System.Collections.Generic;
using System.Diagnostics; // Necesario para Debug

namespace RetoOxxoWeb.Pages
{
    public class LogInModel : PageModel
    {
        [BindProperty]
        public string Nombre { get; set; }
        
        [BindProperty]
        public string Contraseña { get; set; }

        public string Mensaje { get; set; }
        public List<usuario> Usuarios { get; set; } = new List<usuario>();

        public void OnGet()
        {
            DataBaseContext db = new DataBaseContext();
            Usuarios = db.GetAllUsers(); // Obtiene todos los usuarios de la base de datos
        }

        public IActionResult OnPost()
        {
            DataBaseContext db = new DataBaseContext();
            Usuarios = db.GetAllUsers(); // Obtener todos los usuarios de la base de datos

            // Mostrar los datos obtenidos en la consola para verificar
            Debug.WriteLine("Usuarios en la base de datos:");
            foreach (var u in Usuarios)
            {
                Debug.WriteLine($"Usuario: {u.nombre}, Contraseña: {u.contraseña}");
            }

            Debug.WriteLine($"Usuario ingresado: {Nombre}, Contraseña ingresada: {Contraseña}");

            // Verificar si el usuario y la contraseña coinciden
            var usuarioValido = Usuarios.Find(u => u.nombre.Trim() == Nombre.Trim() && u.contraseña.Trim() == Contraseña.Trim());

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
