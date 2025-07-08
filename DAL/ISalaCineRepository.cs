using ProyectoPruebaViamatica.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoPruebaViamatica.DAL.Repositories
{
    public interface ISalaCineRepository
    {
        Task<SalaCine> ObtenerPorId(int id);
        Task<IEnumerable<SalaCine>> ObtenerTodos();
        Task<string> ObtenerDisponibilidadSalaCine(string nombreSalaCine);
        Task<int> CrearSala(SalaCine salaCine);
        Task<bool> ActualizarSala(SalaCine salaCine);
        Task<bool> EliminarSalaLogica(int id);
    }
}