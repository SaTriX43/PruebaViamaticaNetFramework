using System.Collections.Generic;
using System.Threading.Tasks; 
using System.Web.Mvc;
using ProyectoPruebaViamatica.Models.ViewModels; 
using ProyectoPruebaViamatica.Services; 
using System.Linq; 
using System; 

namespace ProyectoPruebaViamatica.Controllers
{
    
    public class PeliculasController : Controller 
    {
        
        private readonly IPeliculaService _peliculaService = new PeliculaService();

       
        public async Task<ActionResult> Index()
        {
            var peliculas = await _peliculaService.ObtenerTodasLasPeliculas();
            return View(peliculas);
        }

        public async Task<ActionResult> Details(int id)
        {
            var pelicula = await _peliculaService.ObtenerPeliculaPorId(id);
            if (pelicula == null)
            {
                return HttpNotFound();
            }
            return View(pelicula);
        }

        public ActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task<ActionResult> Create(PeliculaViewModel peliculaViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _peliculaService.CrearPelicula(peliculaViewModel);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Ocurrió un error al crear la película: " + ex.Message);
                }
            }
            return View(peliculaViewModel);
        }

        public async Task<ActionResult> Edit(int id)
        {
            var pelicula = await _peliculaService.ObtenerPeliculaPorId(id);
            if (pelicula == null)
            {
                return HttpNotFound();
            }
            return View(pelicula);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(PeliculaViewModel peliculaViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _peliculaService.ActualizarPelicula(peliculaViewModel);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Ocurrió un error al actualizar la película: " + ex.Message);
                }
            }
            return View(peliculaViewModel);
        }
        public async Task<ActionResult> Delete(int id)
        {
            var pelicula = await _peliculaService.ObtenerPeliculaPorId(id);
            if (pelicula == null)
            {
                return HttpNotFound();
            }
            return View(pelicula);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _peliculaService.EliminarPelicula(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al eliminar la película: " + ex.Message);
                var pelicula = await _peliculaService.ObtenerPeliculaPorId(id);
                return View("Delete", pelicula);
            }
        }

        public async Task<ActionResult> BuscarPorNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                return RedirectToAction("Index"); 
            }

            var peliculas = await _peliculaService.BuscarPeliculaPorNombre(nombre);
            return View("Index", peliculas); 
        }

        public async Task<ActionResult> PresentarPeliculasPorFechaPublicacion(DateTime fecha)
        {
            var peliculas = await _peliculaService.PresentarPeliculasPorFechaPublicacion(fecha);
            return View("Index", peliculas); 
        }
    }
}