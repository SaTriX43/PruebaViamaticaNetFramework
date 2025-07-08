using ProyectoPruebaViamatica.Models;
using ProyectoPruebaViamatica.Models.ViewModels; 
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoPruebaViamatica.Services
{
    public interface ISalaCineService
    {
        Task<SalaCine> ObtenerSalaCinePorId(int id);
        Task<IEnumerable<SalaCine>> ObtenerTodasLasSalasCine();
        Task<string> ObtenerEstadoSalaCinePorNombre(string nombreSalaCine);
 
    }
}