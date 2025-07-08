using ProyectoPruebaViamatica.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoPruebaViamatica.DAL.Repositories
{
    public interface IPeliculaRepository
    {
        Task<Pelicula> ObtenerPorId(int id);
        Task<IEnumerable<Pelicula>> ObtenerTodos();
        Task Agregar(Pelicula entidad);
        Task Actualizar(Pelicula entidad);
        Task Eliminar(int id); 

        Task<IEnumerable<Pelicula>> BuscarPorNombre(string nombre);
        Task<IEnumerable<Pelicula>> ObtenerPorFechaPublicacion(DateTime fecha);
    }
}