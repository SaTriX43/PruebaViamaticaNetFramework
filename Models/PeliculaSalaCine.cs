using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProyectoPruebaViamatica.Models
{
    using System; // Necesario para DateTime

    public class PeliculaSalacine
    {
        public int id_pelicula_sala { get; set; }    
        public int id_sala_cine { get; set; }      
        public DateTime fecha_publicacion { get; set; }
        public DateTime fecha_fin { get; set; }         
        public int id_pelicula { get; set; }         
}
    }
}