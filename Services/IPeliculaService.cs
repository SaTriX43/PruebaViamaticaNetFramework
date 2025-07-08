using ProyectoPruebaViamatica.Models; 
using ProyectoPruebaViamatica.Models.ViewModels; 
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoPruebaViamatica.Services
{
    public interface IPeliculaService
    {
        Task<PeliculaViewModel> ObtenerPeliculaPorId(int id);
        Task<IEnumerable<PeliculaViewModel>> ObtenerTodasLasPeliculas();
        Task CrearPelicula(PeliculaViewModel peliculaViewModel);
        Task ActualizarPelicula(PeliculaViewModel peliculaViewModel);
        Task EliminarPelicula(int id); 
        Task<IEnumerable<PeliculaViewModel>> BuscarPeliculaPorNombre(string nombre);
        Task<IEnumerable<PeliculaViewModel>> PresentarPeliculasPorFechaPublicacion(DateTime fecha);
    }
}