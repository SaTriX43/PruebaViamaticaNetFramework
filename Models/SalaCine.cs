using System;
using System.ComponentModel.DataAnnotations;

namespace ProyectoPruebaViamatica.Models
{
    public class SalaCine
    {
        public int id_sala { get; set; }
        public string nombre { get; set; }
        public bool estado { get; set; }
    }
}