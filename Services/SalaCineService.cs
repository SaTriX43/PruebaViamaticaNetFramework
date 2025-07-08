using ProyectoPruebaViamatica.DAL.Repositories;
using ProyectoPruebaViamatica.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoPruebaViamatica.Services
{
    public class SalaCineService : ISalaCineService
    {
        private readonly SalaCineRepository _salaCineRepository;

        public SalaCineService()
        {
            _salaCineRepository = new SalaCineRepository();
        }

        public async Task<SalaCine> ObtenerPorId(int id)
        {
            return await _salaCineRepository.ObtenerPorId(id);
        }

        public async Task<IEnumerable<SalaCine>> ObtenerTodasLasSalasCine()
        {
            return await _salaCineRepository.ObtenerTodos();
        }

        public async Task<string> ObtenerEstadoSalaCinePorNombre(string nombreSalaCine)
        {
            return await _salaCineRepository.ObtenerDisponibilidadSalaCine(nombreSalaCine);
        }

        public async Task<int> CrearSala(SalaCine salaCine)
        {
            return await _salaCineRepository.CrearSala(salaCine);
        }

        public async Task<bool> ActualizarSala(SalaCine salaCine)
        {
            return await _salaCineRepository.ActualizarSala(salaCine);
        }

        public async Task<bool> EliminarSalaLogica(int id)
        {
            return await _salaCineRepository.EliminarSalaLogica(id);
        }
    }
}