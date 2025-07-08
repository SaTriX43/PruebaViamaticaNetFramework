using ProyectoPruebaViamatica.Models; 
using System.Threading.Tasks;

namespace ProyectoPruebaViamatica.DAL.Repositories
{
    public interface IPeliculaSalaCineRepository
    {
        Task Agregar(PeliculaSalacine entidad);

    }
}