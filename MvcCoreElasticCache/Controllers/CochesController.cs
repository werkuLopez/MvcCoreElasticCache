using Microsoft.AspNetCore.Mvc;
using MvcCoreElasticCache.Models;
using MvcCoreElasticCache.Repositories;
using MvcCoreElasticCache.Services;

namespace MvcCoreElasticCache.Controllers
{
    public class CochesController : Controller
    {
        private RepositoryCoches repo;
        private ServiceAWSCache service;

        public CochesController(RepositoryCoches repo,
            ServiceAWSCache service)
        {
            this.repo = repo;
            this.service = service;
        }

        public IActionResult Index()
        {
            List<Coche> coches = this.repo.GetCoches();
            return View(coches);
        }

        public IActionResult Details(int id)
        {
            Coche coche = this.repo.GetCoche(id);
            return View(coche);
        }

        public async Task<IActionResult> Favoritos()
        {
            List<Coche> favoritos = await this.service.GetCochesFavoritosAsync();
            return View(favoritos);
        }

        public async Task<IActionResult> SeleccionFavorito(int idcoche)
        {
            // buscamos el coche dentro del xml
            Coche coche = this.repo.GetCoche(idcoche);
            await this.service.AddCocheFavoritoAsync(coche);
            return RedirectToAction("Favoritos");
        }

        public async Task<IActionResult> DeleteFavorito(int idcoche)
        {
            await this.service.ELiminarCocheFavoritoAsync(idcoche);

            return RedirectToAction("Favoritos");
        }
    }
}
