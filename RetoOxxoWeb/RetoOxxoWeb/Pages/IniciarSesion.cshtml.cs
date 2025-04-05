using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RetoOxxoWeb.Model;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace RetoOxxoWeb.Pages
{
    public class LogInModel : PageModel
    {
        [BindProperty]
        public string Nombre { get; set; }
        
        [BindProperty]
        public string Contraseña { get; set; }

        public string Mensaje { get; set; }

        public void OnGet()
        {
            ViewData["Mensaje"] = "";
        }

        public void OnPost()
        {
            DataBaseContext db = new DataBaseContext();
            List<usuario> usuarios = db.GetAllUsers(); // Obtener todos los usuarios de la base de datos

            Debug.WriteLine($"Usuario ingresado: {Nombre}, Contraseña ingresada: {Contraseña}");

            // Verificar si el usuario y la contraseña coinciden
            var usuarioValido = usuarios.Find(u => u.nom_usuario == Nombre && u.contraseña == Contraseña);

            if (usuarioValido != null)
            {
                // Guardar en sesión los datos del usuario
                HttpContext.Session.SetInt32("usuarioID", usuarioValido.id_usuario);

                Debug.WriteLine("Inicio de sesión exitoso. Redirigiendo...");
                Response.Redirect("Index");
            }
            else
            {
                Debug.WriteLine("Credenciales incorrectas.");
                ViewData["Mensaje"] = "Credenciales incorrectas. Inténtalo de nuevo.";
            }
        }
    }
}
