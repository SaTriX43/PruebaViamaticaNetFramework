using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProyectoPruebaViamatica.Models
{
    public class Pelicula
    {
        public int id_pelicula { get; set; } 
        public string nombre { get; set; }   
        public int duracion { get; set; }     
        public bool Activo { get; set; }    
    }
}