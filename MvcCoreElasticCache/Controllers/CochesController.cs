using Microsoft.AspNetCore.Mvc;
using MvcCoreElasticCache.Models;
using MvcCoreElasticCache.Repositories;

namespace MvcCoreElasticCache.Controllers
{
    public class CochesController : Controller
    {
        private RepositoryCoches repo;

        public CochesController(RepositoryCoches repo)
        {
            this.repo = repo;
        }

        public IActionResult Index()
        {
            List<Coche> coches = this.repo.GetCoches();
            return View(coches);
        }

        public IActionResult Details(int id)
        {
            Coche coche= this.repo.GetCoche(id);
            return View(coche);
        }
    }
}
