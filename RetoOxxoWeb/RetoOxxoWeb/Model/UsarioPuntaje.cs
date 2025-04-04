using System;

namespace RetoOxxoWeb.Model
{
    public class UsuarioPuntaje
    {
        public int id_usuario { get; set; }
        public string ?nombre {get; set;}
        public decimal puntos_decision { get; set; }
        public decimal puntos_laberinto { get; set; }
        public decimal puntos_taberna { get; set; }
        public decimal puntos_totales { get; set; }
    }
}