// DAL/Repositories/ISalaCineRepository.cs
using ProyectoPruebaViamatica.Models; // Asegúrate de que esta ruta sea correcta para tu modelo SalaCine
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoPruebaViamatica.DAL.Repositories
{
    public interface ISalaCineRepository
    {
        Task<SalaCine> ObtenerPorId(int id);
        Task<IEnumerable<SalaCine>> ObtenerTodos();
        Task<string> ObtenerDisponibilidadSalaCine(string nombreSalaCine);
    }
}