using ProyectoPruebaViamatica.Models; 
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics; 
using System.Threading.Tasks;

namespace ProyectoPruebaViamatica.DAL.Repositories
{
    public class PeliculaSalaCineRepository : IPeliculaSalaCineRepository
    {
        private readonly string _connectionString;

        public PeliculaSalaCineRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["CineDBConnection"].ConnectionString;
        }

        public async Task Agregar(PeliculaSalacine entidad)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    string query = @"
                        INSERT INTO pelicula_salacine (id_sala_cine, fecha_publicacion, fecha_fin, id_pelicula)
                        VALUES (@id_sala_cine, @fecha_publicacion, @fecha_fin, @id_pelicula)";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@id_sala_cine", entidad.id_sala_cine);
                        cmd.Parameters.AddWithValue("@fecha_publicacion", entidad.fecha_publicacion);
                        cmd.Parameters.AddWithValue("@fecha_fin", entidad.fecha_fin);
                        cmd.Parameters.AddWithValue("@id_pelicula", entidad.id_pelicula);

                        await con.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (SqlException ex)
            {
                Debug.WriteLine($"Error de SQL al agregar asignación de película a sala: {ex.Message}");
                throw; 
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error general al agregar asignación de película a sala: {ex.Message}");
                throw; 
            }
        }
    }
}