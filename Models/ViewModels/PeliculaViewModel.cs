
using System;
using System.ComponentModel.DataAnnotations;

namespace ProyectoPruebaViamatica.Models.ViewModels
{
    public class PeliculaViewModel
    {
        public int id_pelicula { get; set; }

        [Required(ErrorMessage = "El nombre de la película es obligatorio.")]
        [StringLength(255, ErrorMessage = "El nombre no puede exceder los 255 caracteres.")]
        [Display(Name = "Nombre de la Película")]
        public string nombre { get; set; }

        [Required(ErrorMessage = "La duración es obligatoria.")]
        [Range(1, 600, ErrorMessage = "La duración debe estar entre 1 y 600 minutos.")]
        [Display(Name = "Duración (minutos)")]
        public int duracion { get; set; }

        [Display(Name = "Activa")]
        public bool Activo { get; set; }
    }
}