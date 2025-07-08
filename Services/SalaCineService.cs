using ProyectoPruebaViamatica.DAL.Repositories; 
using ProyectoPruebaViamatica.Models; 
using System.Collections.Generic;
using System.Diagnostics; 
using System.Threading.Tasks;
using System;
using System.Linq;

namespace ProyectoPruebaViamatica.Services
{
    public class SalaCineService : ISalaCineService
    {
        private readonly ISalaCineRepository _salaCineRepo;

        public SalaCineService()
        {
            _salaCineRepo = new SalaCineRepository();
        }

        public async Task<SalaCine> ObtenerSalaCinePorId(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentException("El ID de la sala de cine debe ser un valor positivo.", nameof(id));
                }
                return await _salaCineRepo.ObtenerPorId(id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error en SalaCineService.ObtenerSalaCinePorId: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<SalaCine>> ObtenerTodasLasSalasCine()
        {
            try
            {
                return await _salaCineRepo.ObtenerTodos();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error en SalaCineService.ObtenerTodasLasSalasCine: {ex.Message}");
                throw;
            }
        }

        public async Task<string> ObtenerEstadoSalaCinePorNombre(string nombreSalaCine)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nombreSalaCine))
                {
                    throw new ArgumentException("El nombre de la sala de cine es obligatorio para verificar su estado.");
                }
                return await _salaCineRepo.ObtenerDisponibilidadSalaCine(nombreSalaCine);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error en SalaCineService.ObtenerEstadoSalaCinePorNombre: {ex.Message}");
                throw;
            }
        }
    }
}