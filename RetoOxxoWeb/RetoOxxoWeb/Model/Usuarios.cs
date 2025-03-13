using System;

namespace RetoOxxoWeb.Model
{
    public class usuario
    {
        public int id_usuario {get;set;}
        public string nombre {get;set;}
        public string apellidop {get;set;}
        public string apellidom {get;set;}
        public string telefono {get;set;}
        public string fotografia {get;set;}
        public int codigo_postal {get;set;}
        public string calle {get;set;}
        public string estado {get;set;}
        public string ciudad {get;set;}
        public byte tipoempleado {get;set;} 
    }
}