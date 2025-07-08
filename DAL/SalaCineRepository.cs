using ProyectoPruebaViamatica.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ProyectoPruebaViamatica.DAL.Repositories
{
    public class SalaCineRepository : ISalaCineRepository
    {
        private readonly string _connectionString;

        public SalaCineRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["CineDBConnection"].ConnectionString;
        }

        private SalaCine MapearSalaCineDesdeLector(SqlDataReader lector)
        {
            return new SalaCine
            {
                id_sala = Convert.ToInt32(lector["id_sala"]),
                nombre = lector["nombre"].ToString(),
                estado = lector.GetBoolean(lector.GetOrdinal("estado"))
            };
        }

        public async Task<SalaCine> ObtenerPorId(int id)
        {
            SalaCine salaCine = null;
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    string query = "SELECT id_sala, nombre, estado FROM sala_cine WHERE id_sala = @id AND estado = 1";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        await con.OpenAsync();
                        using (SqlDataReader lector = await cmd.ExecuteReaderAsync())
                        {
                            if (await lector.ReadAsync())
                            {
                                salaCine = MapearSalaCineDesdeLector(lector);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Debug.WriteLine($"Error de SQL al obtener sala de cine por ID: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error general al obtener sala de cine por ID: {ex.Message}");
                throw;
            }
            return salaCine;
        }

        public async Task<IEnumerable<SalaCine>> ObtenerTodos()
        {
            List<SalaCine> salasCine = new List<SalaCine>();
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    string query = "SELECT id_sala, nombre, estado FROM sala_cine WHERE estado = 1";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        await con.OpenAsync();
                        using (SqlDataReader lector = await cmd.ExecuteReaderAsync())
                        {
                            while (await lector.ReadAsync())
                            {
                                salasCine.Add(MapearSalaCineDesdeLector(lector));
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Debug.WriteLine($"Error de SQL al obtener todas las salas de cine: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error general al obtener todas las salas de cine: {ex.Message}");
                throw;
            }
            return salasCine;
        }

        public async Task<string> ObtenerDisponibilidadSalaCine(string nombreSalaCine)
        {
            if (string.IsNullOrWhiteSpace(nombreSalaCine))
            {
                return "Nombre de sala no proporcionado.";
            }

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    string query = @"
                        SELECT
                            sc.estado AS SalaEstado,
                            COUNT(DISTINCT ps.id_pelicula) AS PeliculasAsignadasCount
                        FROM
                            sala_cine sc
                        LEFT JOIN
                            pelicula_salacine ps ON sc.id_sala = ps.id_sala_cine
                        LEFT JOIN
                            pelicula p ON ps.id_pelicula = p.id_pelicula
                        WHERE
                            sc.nombre = @nombreSalaCine AND sc.estado = 1
                            AND (
                                p.Activo = 1
                                AND GETDATE() BETWEEN ps.fecha_publicacion AND ps.fecha_fin
                                OR ps.id_pelicula IS NULL
                            )
                        GROUP BY sc.estado;";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@nombreSalaCine", nombreSalaCine);
                        await con.OpenAsync();

                        using (SqlDataReader lector = await cmd.ExecuteReaderAsync())
                        {
                            if (await lector.ReadAsync())
                            {
                                bool salaEstado = Convert.ToBoolean(lector["SalaEstado"]);
                                int peliculasAsignadasCount = Convert.ToInt32(lector["PeliculasAsignadasCount"]);

                                if (!salaEstado)
                                {
                                    return $"La sala '{nombreSalaCine}' está inactiva.";
                                }

                                if (peliculasAsignadasCount < 3)
                                {
                                    return $"Sala disponible";
                                }
                                else if (peliculasAsignadasCount >= 3 && peliculasAsignadasCount <= 5)
                                {
                                    return $"Sala con {peliculasAsignadasCount} películas asignadas";
                                }
                                else
                                {
                                    return $"Sala no disponible";
                                }
                            }
                            else
                            {
                                return $"La sala '{nombreSalaCine}' no fue encontrada o no está activa.";
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Debug.WriteLine($"Error de SQL al obtener disponibilidad de sala de cine: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error general al obtener disponibilidad de sala de cine: {ex.Message}");
                throw;
            }
        }

        public async Task<int> CrearSala(SalaCine salaCine)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    string query = "INSERT INTO sala_cine (nombre, estado) VALUES (@nombre, @estado); SELECT SCOPE_IDENTITY();";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@nombre", salaCine.nombre);
                        cmd.Parameters.AddWithValue("@estado", salaCine.estado);

                        await con.OpenAsync();
                        int newId = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                        return newId;
                    }
                }
            }
            catch (SqlException ex)
            {
                Debug.WriteLine($"Error de SQL al crear sala de cine: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error general al crear sala de cine: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> ActualizarSala(SalaCine salaCine)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    string query = "UPDATE sala_cine SET nombre = @nombre, estado = @estado WHERE id_sala = @id_sala";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@nombre", salaCine.nombre);
                        cmd.Parameters.AddWithValue("@estado", salaCine.estado);
                        cmd.Parameters.AddWithValue("@id_sala", salaCine.id_sala);

                        await con.OpenAsync();
                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                Debug.WriteLine($"Error de SQL al actualizar sala de cine: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error general al actualizar sala de cine: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> EliminarSalaLogica(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    string query = "UPDATE sala_cine SET estado = 0 WHERE id_sala = @id_sala";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@id_sala", id);

                        await con.OpenAsync();
                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                Debug.WriteLine($"Error de SQL al eliminar sala de cine lógicamente: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error general al eliminar sala de cine lógicamente: {ex.Message}");
                throw;
            }
        }
    }
}