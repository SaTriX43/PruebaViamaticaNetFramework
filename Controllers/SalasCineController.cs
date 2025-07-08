using ProyectoPruebaViamatica.Models;
using ProyectoPruebaViamatica.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

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

        public ActionResult Create()
        {
            return View();
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(SalaCine salaCine)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int newId = await _salaCineService.CrearSala(salaCine);
                    if (newId > 0)
                    {
                        return RedirectToAction("Index");
                    }
                    ModelState.AddModelError("", "No se pudo crear la sala de cine.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en SalasCineController.Create (POST): {ex.Message}");
                ModelState.AddModelError("", $"Ocurrió un error al crear la sala: {ex.Message}");
            }
            return View(salaCine);
        }

        
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalaCine salaCine = await _salaCineService.ObtenerPorId(id.Value);
            if (salaCine == null)
            {
                return HttpNotFound();
            }
            return View(salaCine);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(SalaCine salaCine)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool updated = await _salaCineService.ActualizarSala(salaCine);
                    if (updated)
                    {
                        return RedirectToAction("Index");
                    }
                    ModelState.AddModelError("", "No se pudo actualizar la sala de cine o no se encontró.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en SalasCineController.Edit (POST): {ex.Message}");
                ModelState.AddModelError("", $"Ocurrió un error al actualizar la sala: {ex.Message}");
            }
            return View(salaCine);
        }

        
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalaCine salaCine = await _salaCineService.ObtenerPorId(id.Value);
            if (salaCine == null)
            {
                return HttpNotFound();
            }
            return View(salaCine);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                bool deleted = await _salaCineService.EliminarSalaLogica(id);
                if (deleted)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "No se pudo eliminar la sala de cine o no se encontró.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en SalasCineController.DeleteConfirmed: {ex.Message}");
                ModelState.AddModelError("", $"Ocurrió un error al eliminar la sala: {ex.Message}");
            }
            return RedirectToAction("Index");
        }
    }
}