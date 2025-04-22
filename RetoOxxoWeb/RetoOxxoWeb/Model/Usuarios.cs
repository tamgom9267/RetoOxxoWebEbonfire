using System;
using System.ComponentModel.DataAnnotations;

namespace RetoOxxoWeb.Model
{
    public class usuario
    {
        public int id_usuario { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        [StringLength(20, ErrorMessage = "El nombre de usuario debe tener al menos 6 caracteres", MinimumLength = 8)]
        public string nom_usuario {get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(20, ErrorMessage = "El nombre debe tener al menos 6 caracteres", MinimumLength = 6)]
        public string nombre { get; set; }

        [Required(ErrorMessage = "El apellido paterno es obligatorio")]
        [StringLength(20, ErrorMessage = "El apellido paterno debe tener al menos 6 caracteres", MinimumLength = 6)]
        public string apellidop { get; set; }

        [Required(ErrorMessage = "El apellido materno es obligatorio")]
        [StringLength(20, ErrorMessage = "El apellido materno debe tener al menos 6 caracteres", MinimumLength = 6)]
        public string apellidom { get; set; }

        [Required(ErrorMessage = "El telefono es obligatorio")]
        [StringLength(10, ErrorMessage = "El telefono debe tener al menos 10 caracteres", MinimumLength = 10)]
        public string telefono { get; set; }

        [Required(ErrorMessage = "La imagen es obligatorio")]
        [StringLength(100, ErrorMessage = "La imagen debe tener al menos 10 caracteres", MinimumLength = 10)]
        public string fotografia { get; set; }


        [Required(ErrorMessage = "El tipo de empleado es obligatorio")]
        [Range(0, 1, ErrorMessage = "El tipo de empleado debe ser 0 o 1")]
        public byte tipo_empleado { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(20, ErrorMessage = "La contraseña debe tener al menos 6 caracteres", MinimumLength = 6)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{6,}$", ErrorMessage = "La contraseña debe contener al menos una letra mayúscula, una letra minúscula y un número")]
        public string contraseña { get; set; }

        [Required(ErrorMessage = "El codigo postal es obligatorio")]
        [Range(10000, 10004, ErrorMessage = "El codigo postal debe ser un número de 5 dígitos")]
        public int? cp { get; set; }

        [Required(ErrorMessage = "La calle es obligatoria")]
        [StringLength(50, ErrorMessage = "La calle debe tener al menos 6 caracteres", MinimumLength = 6)]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "La calle solo puede contener letras y espacios")]
        public string calle { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio")]
        [StringLength(50, ErrorMessage = "El estado debe tener al menos 6 caracteres", MinimumLength = 6)]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "El estado solo puede contener letras y espacios")]
        public string estado { get; set; }

        [Required(ErrorMessage = "La ciudad es obligatoria")]
        [StringLength(50, ErrorMessage = "La ciudad debe tener al menos 6 caracteres", MinimumLength = 6)]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "La ciudad solo puede contener letras y espacios")]
        public string ciudad { get; set; }
    
        public usuario()
        {
            
        }
        public usuario(int id_usuario_, string nombre_, string contraseña_)
        {
            this.id_usuario = id_usuario_;
            this.nombre = nombre_;
            this.contraseña = contraseña_;
            
        }

    }

    
}
