using ProyectoPruebaViamatica.Models.ViewModels;
using ProyectoPruebaViamatica.Services;
using System.Threading.Tasks;
using System.Web.Mvc;
using System;

namespace ProyectoPruebaViamatica.Controllers
{
    public class PeliculaSalaCineController : Controller
    {
        private readonly IPeliculaSalaCineService _peliculaSalaCineService = new PeliculaSalaCineService();

        public ActionResult Asignar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Asignar(AsignarPeliculaSalaViewModel asignacionViewModel)
        {
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