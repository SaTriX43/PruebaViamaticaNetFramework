using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc; 

namespace ProyectoPruebaViamatica.Models.ViewModels
{
    public class AsignarPeliculaSalaViewModel
    {
        [Required(ErrorMessage = "Debe seleccionar una película.")]
        [Display(Name = "Película")]
        public int id_pelicula { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una sala de cine.")]
        [Display(Name = "Sala de Cine")]
        public int id_sala_cine { get; set; }

        [Required(ErrorMessage = "La fecha de publicación es obligatoria.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de Publicación")]
        public DateTime fecha_publicacion { get; set; }

        [Required(ErrorMessage = "La fecha de fin es obligatoria.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de Fin")]
        public DateTime fecha_fin { get; set; }

        public SelectList PeliculasDisponibles { get; set; }
        public SelectList SalasDisponibles { get; set; }
    }
}