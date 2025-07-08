using ProyectoPruebaViamatica.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ProyectoPruebaViamatica.DAL.Repositories
{
    public class PeliculaRepository : IPeliculaRepository
    {
        private readonly string _connectionString;

        public PeliculaRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["CineDBConnection"].ConnectionString;
        }

        //metodo para mapear  los datos del lector a un objeto Pelicula
        private Pelicula MapearPeliculaDesdeLector(SqlDataReader lector)
        {
            return new Pelicula
            {
                id_pelicula = Convert.ToInt32(lector["id_pelicula"]),
                nombre = lector["nombre"].ToString(),
                duracion = Convert.ToInt32(lector["duracion"]),
                Activo = Convert.ToBoolean(lector["Activo"])
            };
        }

        public async Task<Pelicula> ObtenerPorId(int id)
        {
            Pelicula pelicula = null;
            try 
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    string query = "SELECT id_pelicula, nombre, duracion, Activo FROM pelicula WHERE id_pelicula = @id AND Activo = 1";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        await con.OpenAsync();
                        using (SqlDataReader lector = await cmd.ExecuteReaderAsync())
                        {
                            if (await lector.ReadAsync())
                            {
                                pelicula = MapearPeliculaDesdeLector(lector);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex) 
            {
                
                Debug.WriteLine($"Error de SQL al obtener película por ID: {ex.Message}");
                throw; 
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error general al obtener película por ID: {ex.Message}");
                throw; 
            }
            return pelicula;
        }

        public async Task<IEnumerable<Pelicula>> ObtenerTodos()
        {
            List<Pelicula> peliculas = new List<Pelicula>();
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    string query = "SELECT id_pelicula, nombre, duracion, Activo FROM pelicula WHERE Activo = 1";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        await con.OpenAsync();
                        using (SqlDataReader lector = await cmd.ExecuteReaderAsync())
                        {
                            while (await lector.ReadAsync())
                            {
                                peliculas.Add(MapearPeliculaDesdeLector(lector));
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Debug.WriteLine($"Error de SQL al obtener todas las películas: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error general al obtener todas las películas: {ex.Message}");
                throw;
            }
            return peliculas;
        }

        public async Task Agregar(Pelicula entidad)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    string query = "INSERT INTO pelicula (nombre, duracion, Activo) VALUES (@nombre, @duracion, @activo)";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@nombre", entidad.nombre);
                        cmd.Parameters.AddWithValue("@duracion", entidad.duracion);
                        cmd.Parameters.AddWithValue("@activo", true);
                        await con.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (SqlException ex)
            {
                Debug.WriteLine($"Error de SQL al agregar película: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error general al agregar película: {ex.Message}");
                throw;
            }
        }

        public async Task Actualizar(Pelicula entidad)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    string query = "UPDATE pelicula SET nombre = @nombre, duracion = @duracion, Activo = @activo WHERE id_pelicula = @id";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@nombre", entidad.nombre);
                        cmd.Parameters.AddWithValue("@duracion", entidad.duracion);
                        cmd.Parameters.AddWithValue("@activo", entidad.Activo);
                        cmd.Parameters.AddWithValue("@id", entidad.id_pelicula);
                        await con.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (SqlException ex)
            {
                Debug.WriteLine($"Error de SQL al actualizar película: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error general al actualizar película: {ex.Message}");
                throw;
            }
        }

        public async Task Eliminar(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    string query = "UPDATE pelicula SET Activo = 0 WHERE id_pelicula = @id";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        await con.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (SqlException ex)
            {
                Debug.WriteLine($"Error de SQL al eliminar película: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error general al eliminar película: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Pelicula>> BuscarPorNombre(string nombre)
        {
            List<Pelicula> peliculas = new List<Pelicula>();
            if (string.IsNullOrWhiteSpace(nombre))
            {
                return peliculas;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    
                    using (SqlCommand cmd = new SqlCommand("usp_BuscarPeliculaPorNombre", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure; 
                        cmd.Parameters.AddWithValue("@NombrePelicula", nombre); 
                        await con.OpenAsync();
                        using (SqlDataReader lector = await cmd.ExecuteReaderAsync())
                        {
                            while (await lector.ReadAsync())
                            {
                                peliculas.Add(MapearPeliculaDesdeLector(lector));
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Debug.WriteLine($"Error de SQL al buscar películas por nombre: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error general al buscar películas por nombre: {ex.Message}");
                throw;
            }
            return peliculas;
        }

        public async Task<IEnumerable<Pelicula>> ObtenerPorFechaPublicacion(DateTime fecha)
        {
            List<Pelicula> peliculas = new List<Pelicula>();
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_GetPeliculasByFechaPublicacion", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@fecha", fecha.Date);
                        await con.OpenAsync();
                        using (SqlDataReader lector = await cmd.ExecuteReaderAsync())
                        {
                            while (await lector.ReadAsync())
                            {
                                peliculas.Add(MapearPeliculaDesdeLector(lector));
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Debug.WriteLine($"Error de SQL al obtener películas por fecha de publicación (SP): {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error general al obtener películas por fecha de publicación (SP): {ex.Message}");
                throw;
            }
            return peliculas;
        }
    }
}