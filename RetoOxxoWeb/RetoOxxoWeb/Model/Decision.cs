using System;
using Org.BouncyCastle.Asn1.Cms;

namespace RetoOxxoWeb.Model
{
    public class decision
    {
        public int id_decision {get; set;}
        public int puntos {get; set;}
        public Time ?tiempo {get; set;}
        public int id_usuario {get; set;}
    }
}