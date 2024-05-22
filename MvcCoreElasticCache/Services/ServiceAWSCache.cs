using Microsoft.Extensions.Caching.Distributed;
using MvcCoreElasticCache.Models;
using Newtonsoft.Json;

namespace MvcCoreElasticCache.Services
{
    public class ServiceAWSCache
    {
        private IDistributedCache cache;

        public ServiceAWSCache(IDistributedCache cache)
        {
            this.cache = cache;
        }

        public async Task<List<Coche>> GetCochesFavoritosAsync()
        {
            // almacenaremos una colección de coches en formato json
            string json = await this.cache.GetStringAsync("cochesfavoritos");
            if (json == null)
            {
                return null;
            }
            else
            {
                List<Coche> coches = JsonConvert.DeserializeObject<List<Coche>>(json);
                return coches;
            }
        }

        public async Task AddCocheFavoritoAsync(Coche coche)
        {
            List<Coche> coches = await this.GetCochesFavoritosAsync();
            if (coches == null)
            {
                coches = new List<Coche>();
            }
            // añadimos el nuevo coche a la colección
            coches.Add(coche);

            string json = JsonConvert.SerializeObject(coches);

            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(30),
            };


            await this.cache.SetStringAsync("cochesfavoritos", json, options);
        }

        public async Task ELiminarCocheFavoritoAsync(int idcoche)
        {
            List<Coche> coches = await this.GetCochesFavoritosAsync();

            if (coches != null)
            {
                Coche car = coches.FirstOrDefault(x => x.IdCoche == idcoche);

                coches.Remove(car);

                // si no hay coches en favoritos, eliminamos la clave
                if (coches.Count == 0)
                {
                    await this.cache.RemoveAsync("cochesfavoritos");
                }
                else
                {
                    // almacenamos los coches
                    string json = JsonConvert.SerializeObject(coches);

                    DistributedCacheEntryOptions options = new DistributedCacheEntryOptions
                    {
                        SlidingExpiration = TimeSpan.FromMinutes(30),
                    };

                    //actualizamos la colección
                    await this.cache.SetStringAsync("cochesfavoritos", json, options);
                }
            }
        }
    }
}
