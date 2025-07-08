using ProyectoPruebaViamatica.Models.ViewModels; 
using System.Threading.Tasks;

namespace ProyectoPruebaViamatica.Services
{
    public interface IPeliculaSalaCineService
    {
        Task CrearPeliculaSalaCine(AsignarPeliculaSalaViewModel asignacionViewModel);
    }
}