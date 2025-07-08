using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProyectoPruebaViamatica.Models
{
    public class SalaCine
    {
        public int id_sala { get; set; }    
        public string nombre { get; set; }  
        public bool estado { get; set; }     // Estado de la sala de cine (ej. 1=Activa, 0=Inactiva/Eliminada lógicamente)
    }
}
