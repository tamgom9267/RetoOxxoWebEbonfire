using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RetoOxxoWeb.Model;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace RetoOxxoWeb.Pages
{
    public class SignInModel : PageModel
    {
        [BindProperty]
        public string Usuario {get; set; }

        [BindProperty]
        public string Nombre {get; set; }

        [BindProperty]
        public string apellidoP {get; set; }

        [BindProperty]
        public string apellidoM {get; set; }

        [BindProperty]
        public string Contraseña {get; set; }

        public string Mensaje_registro { get; set; }

        public void OnGet()
        {
            ViewData["Mensaje_registro"] = "";
        }

        public void OnPost()
        {
            DataBaseContext db = new DataBaseContext();
            List<usuario> usuarios = db.GetAllUsers(); 
            Debug.WriteLine($"Total usuarios en la base de datos: {usuarios.Count}");

            foreach (var u in usuarios)
            {
                Debug.WriteLine($"Usuario en BD: '{u.nom_usuario}'");
            }


            Debug.WriteLine($"Usuario ingresado: {Usuario}, Contraseña ingresada: {Contraseña}");

            // Verificar si el usuario existe
            var usuarioValido = usuarios.Find(u => u.nom_usuario == Usuario);

            if (usuarioValido != null)
            {
                Debug.WriteLine("Usuario Repetido.");
                Mensaje_registro = "Este nombre de usuario ya está en uso. Inténtalo de nuevo con un nombre diferente.";
                return;
            } else {
                //HttpContext.Session.SetInt32("usuarioID", usuarioValido.id_usuario);       
                
            }
            Debug.WriteLine("Cuenta creada con éxito. Redirigiendo...");
            Response.Redirect("Index");
        }
    }
}
