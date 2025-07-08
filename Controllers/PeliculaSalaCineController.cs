using ProyectoPruebaViamatica.Models.ViewModels;
using ProyectoPruebaViamatica.Services;
using System.Threading.Tasks;
using System.Web.Mvc;
using System;
using System.Linq;
using System.Collections.Generic;

namespace ProyectoPruebaViamatica.Controllers
{
    public class PeliculaSalaCineController : Controller
    {
        private readonly IPeliculaSalaCineService _peliculaSalaCineService;
        private readonly IPeliculaService _peliculaService;
        private readonly ISalaCineService _salaCineService;

        public PeliculaSalaCineController()
        {
            _peliculaSalaCineService = new PeliculaSalaCineService();
            _peliculaService = new PeliculaService();
            _salaCineService = new SalaCineService();
        }

        public async Task<ActionResult> Asignar()
        {
            var viewModel = new AsignarPeliculaSalaViewModel();

            var peliculasActivas = await _peliculaService.ObtenerTodasLasPeliculas();
            viewModel.PeliculasDisponibles = new SelectList(peliculasActivas, "id_pelicula", "nombre");

            var salasActivas = await _salaCineService.ObtenerTodasLasSalasCine();
            viewModel.SalasDisponibles = new SelectList(salasActivas, "id_sala", "nombre");

            viewModel.fecha_publicacion = DateTime.Today;
            viewModel.fecha_fin = DateTime.Today.AddDays(7);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Asignar(AsignarPeliculaSalaViewModel asignacionViewModel)
        {
            var peliculasActivas = await _peliculaService.ObtenerTodasLasPeliculas();
            asignacionViewModel.PeliculasDisponibles = new SelectList(peliculasActivas, "id_pelicula", "nombre", asignacionViewModel.id_pelicula);

            var salasActivas = await _salaCineService.ObtenerTodasLasSalasCine();
            asignacionViewModel.SalasDisponibles = new SelectList(salasActivas, "id_sala", "nombre", asignacionViewModel.id_sala_cine);

            if (!ModelState.IsValid)
            {
                return View(asignacionViewModel);
            }

            try
            {
                await _peliculaSalaCineService.CrearPeliculaSalaCine(asignacionViewModel);
                TempData["SuccessMessage"] = "La asignación de película a sala se creó exitosamente.";
                return RedirectToAction("Confirmacion");
            }
            catch (ArgumentException argEx)
            {
                ModelState.AddModelError("", "Error de datos: " + argEx.Message);
                return View(asignacionViewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en PeliculaSalaCineController.Asignar (POST): {ex.Message}");
                ModelState.AddModelError("", "Ocurrió un error inesperado al asignar la película a la sala. " + ex.Message);
                return View(asignacionViewModel);
            }
        }

        public ActionResult Confirmacion()
        {
            ViewBag.Message = TempData["SuccessMessage"] as string;
            return View();
        }
    }
}