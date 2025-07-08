using ProyectoPruebaViamatica.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoPruebaViamatica.Services
{
    public interface ISalaCineService
    {
        Task<SalaCine> ObtenerPorId(int id);
        Task<IEnumerable<SalaCine>> ObtenerTodasLasSalasCine();
        Task<string> ObtenerEstadoSalaCinePorNombre(string nombreSalaCine);
        Task<int> CrearSala(SalaCine salaCine);
        Task<bool> ActualizarSala(SalaCine salaCine);
        Task<bool> EliminarSalaLogica(int id);
    }
}