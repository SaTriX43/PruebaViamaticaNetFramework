using ProyectoPruebaViamatica.Models;
using ProyectoPruebaViamatica.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using System;

namespace ProyectoPruebaViamatica.Controllers
{
    public class SalasCineController : Controller
    {
        private readonly ISalaCineService _salaCineService = new SalaCineService();

        public async Task<ActionResult> Index()
        {
            try
            {
                var salas = await _salaCineService.ObtenerTodasLasSalasCine();
                return View(salas);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en SalasCineController.Index: {ex.Message}");
                ViewBag.ErrorMessage = "No se pudieron cargar las salas de cine.";
                return View(new List<SalaCine>());
            }
        }

        public async Task<ActionResult> Disponibilidad(string nombreSalaCine)
        {
            if (string.IsNullOrWhiteSpace(nombreSalaCine))
            {
                ViewBag.ErrorMessage = "Por favor, ingrese el nombre de una sala de cine para verificar su disponibilidad.";
                return View();
            }

            string estadoDisponibilidad = null;
            try
            {
                estadoDisponibilidad = await _salaCineService.ObtenerEstadoSalaCinePorNombre(nombreSalaCine);

                if (string.IsNullOrEmpty(estadoDisponibilidad))
                {
                    ViewBag.ErrorMessage = $"No se encontró información de disponibilidad para la sala '{nombreSalaCine}' o la sala no existe.";
                    return View();
                }

                ViewBag.NombreSalaCine = nombreSalaCine;
                ViewBag.EstadoDisponibilidad = estadoDisponibilidad;
                return View();
            }
            catch (ArgumentException argEx)
            {
                ViewBag.ErrorMessage = "Error en el nombre de la sala: " + argEx.Message;
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en SalasCineController.Disponibilidad: {ex.Message}");
                ViewBag.ErrorMessage = "Ocurrió un error inesperado al verificar la disponibilidad. Por favor, intente de nuevo más tarde.";
                return View();
            }
        }
    }
}