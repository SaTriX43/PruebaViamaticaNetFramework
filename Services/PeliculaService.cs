using ProyectoPruebaViamatica.DAL.Repositories; 
using ProyectoPruebaViamatica.Models;
using ProyectoPruebaViamatica.Models.ViewModels; 
using System;
using System.Collections.Generic;
using System.Diagnostics; 
using System.Linq; 
using System.Threading.Tasks;

namespace ProyectoPruebaViamatica.Services
{
    public class PeliculaService : IPeliculaService
    {
        private readonly IPeliculaRepository _peliculaRepo;

        public PeliculaService()
        {
            _peliculaRepo = new PeliculaRepository();
        }

        private PeliculaViewModel MapearADTO(Pelicula pelicula)
        {
            if (pelicula == null) return null;
            return new PeliculaViewModel
            {
                id_pelicula = pelicula.id_pelicula,
                nombre = pelicula.nombre,
                duracion = pelicula.duracion,
                Activo = pelicula.Activo
            };
        }

        private Pelicula MapearAEntidad(PeliculaViewModel peliculaViewModel)
        {
            if (peliculaViewModel == null) return null;
            return new Pelicula
            {
                id_pelicula = peliculaViewModel.id_pelicula,
                nombre = peliculaViewModel.nombre,
                duracion = peliculaViewModel.duracion,
                Activo = peliculaViewModel.Activo 
            };
        }

        public async Task<PeliculaViewModel> ObtenerPeliculaPorId(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentException("El ID de la película debe ser un valor positivo.", nameof(id));
                }

                var pelicula = await _peliculaRepo.ObtenerPorId(id);
                return MapearADTO(pelicula);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error en PeliculaService.ObtenerPeliculaPorId: {ex.Message}");
                throw; 
            }
        }

        public async Task<IEnumerable<PeliculaViewModel>> ObtenerTodasLasPeliculas()
        {
            try
            {
                var peliculas = await _peliculaRepo.ObtenerTodos();
                return peliculas.Select(p => MapearADTO(p)).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error en PeliculaService.ObtenerTodasLasPeliculas: {ex.Message}");
                throw;
            }
        }

        public async Task CrearPelicula(PeliculaViewModel peliculaViewModel)
        {
            try
            {
                if (peliculaViewModel == null)
                {
                    throw new ArgumentNullException(nameof(peliculaViewModel), "Los datos de la película no pueden ser nulos.");
                }
                if (string.IsNullOrWhiteSpace(peliculaViewModel.nombre))
                {
                    throw new ArgumentException("El nombre de la película es obligatorio.");
                }
                if (peliculaViewModel.duracion <= 0)
                {
                    throw new ArgumentException("La duración de la película debe ser mayor a cero.");
                }

                var pelicula = MapearAEntidad(peliculaViewModel);
                await _peliculaRepo.Agregar(pelicula);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error en PeliculaService.CrearPelicula: {ex.Message}");
                throw;
            }
        }

        public async Task ActualizarPelicula(PeliculaViewModel peliculaViewModel)
        {
            try
            {
                if (peliculaViewModel == null)
                {
                    throw new ArgumentNullException(nameof(peliculaViewModel), "Los datos de la película no pueden ser nulos.");
                }
                if (peliculaViewModel.id_pelicula <= 0)
                {
                    throw new ArgumentException("El ID de la película es obligatorio para la actualización.");
                }
                if (string.IsNullOrWhiteSpace(peliculaViewModel.nombre))
                {
                    throw new ArgumentException("El nombre de la película es obligatorio.");
                }
                if (peliculaViewModel.duracion <= 0)
                {
                    throw new ArgumentException("La duración de la película debe ser mayor a cero.");
                }

                var peliculaExistente = await _peliculaRepo.ObtenerPorId(peliculaViewModel.id_pelicula);
                if (peliculaExistente == null)
                {
                    throw new InvalidOperationException($"La película con ID {peliculaViewModel.id_pelicula} no existe o no está activa.");
                }

                var pelicula = MapearAEntidad(peliculaViewModel);
                await _peliculaRepo.Actualizar(pelicula);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error en PeliculaService.ActualizarPelicula: {ex.Message}");
                throw;
            }
        }

        public async Task EliminarPelicula(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentException("El ID de la película debe ser un valor positivo para la eliminación.", nameof(id));
                }

                var peliculaExistente = await _peliculaRepo.ObtenerPorId(id);
                if (peliculaExistente == null)
                {
                    throw new InvalidOperationException($"La película con ID {id} no existe o ya está inactiva.");
                }

                await _peliculaRepo.Eliminar(id); 
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error en PeliculaService.EliminarPelicula: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<PeliculaViewModel>> BuscarPeliculaPorNombre(string nombre)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nombre))
                {
                    return new List<PeliculaViewModel>();
                }

                var peliculas = await _peliculaRepo.BuscarPorNombre(nombre);
                return peliculas.Select(p => MapearADTO(p)).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error en PeliculaService.BuscarPeliculaPorNombre: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<PeliculaViewModel>> PresentarPeliculasPorFechaPublicacion(DateTime fecha)
        {
            try
            {
                if (fecha == default(DateTime) && fecha.Kind == DateTimeKind.Unspecified)
                {
                    throw new ArgumentException("La fecha de publicación proporcionada no es válida.");
                }

                var peliculas = await _peliculaRepo.ObtenerPorFechaPublicacion(fecha);
                return peliculas.Select(p => MapearADTO(p)).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error en PeliculaService.PresentarPeliculasPorFechaPublicacion: {ex.Message}");
                throw;
            }
        }
    }
}