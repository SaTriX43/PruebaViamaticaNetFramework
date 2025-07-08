
using ProyectoPruebaViamatica.DAL.Repositories; 
using ProyectoPruebaViamatica.Models;
using ProyectoPruebaViamatica.Models.ViewModels; 
using System;
using System.Diagnostics; 
using System.Threading.Tasks;

namespace ProyectoPruebaViamatica.Services
{
    public class PeliculaSalaCineService : IPeliculaSalaCineService
    {
        private readonly IPeliculaSalaCineRepository _peliculaSalaCineRepo;

        public PeliculaSalaCineService()
        {
            _peliculaSalaCineRepo = new PeliculaSalaCineRepository();
        }

        private PeliculaSalacine MapearAEntidad(AsignarPeliculaSalaViewModel asignacionViewModel)
        {
            if (asignacionViewModel == null) return null;
            return new PeliculaSalacine
            {
                id_pelicula = asignacionViewModel.id_pelicula,
                id_sala_cine = asignacionViewModel.id_sala_cine,
                fecha_publicacion = asignacionViewModel.fecha_publicacion,
                fecha_fin = asignacionViewModel.fecha_fin
            };
        }

        public async Task CrearPeliculaSalaCine(AsignarPeliculaSalaViewModel asignacionViewModel)
        {
            try
            {
                
                if (asignacionViewModel == null)
                {
                    throw new ArgumentNullException(nameof(asignacionViewModel), "Los datos de la asignación no pueden ser nulos.");
                }
                if (asignacionViewModel.id_pelicula <= 0)
                {
                    throw new ArgumentException("Debe seleccionar una película válida.");
                }
                if (asignacionViewModel.id_sala_cine <= 0)
                {
                    throw new ArgumentException("Debe seleccionar una sala de cine válida.");
                }
                if (asignacionViewModel.fecha_publicacion == default(DateTime))
                {
                    throw new ArgumentException("La fecha de publicación es obligatoria.");
                }
                if (asignacionViewModel.fecha_fin == default(DateTime))
                {
                    throw new ArgumentException("La fecha de fin es obligatoria.");
                }
                if (asignacionViewModel.fecha_fin < asignacionViewModel.fecha_publicacion)
                {
                    throw new ArgumentException("La fecha de fin no puede ser anterior a la fecha de publicación.");
                }

                var asignacionEntidad = MapearAEntidad(asignacionViewModel);
                await _peliculaSalaCineRepo.Agregar(asignacionEntidad);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error en PeliculaSalaCineService.CrearPeliculaSalaCine: {ex.Message}");
                throw; 
            }
        }
    }
}