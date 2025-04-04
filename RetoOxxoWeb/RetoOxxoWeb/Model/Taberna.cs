using System;
using Org.BouncyCastle.Asn1.Cms;

namespace RetoOxxoWeb.Model
{
    public class taberna
    {
        public int id_taberna {get; set;}
        public int puntos {get; set;}
        public Time ?tiempo {get; set;}
        public int id_usuario {get; set;}
    }
}