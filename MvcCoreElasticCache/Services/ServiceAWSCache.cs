using MvcCoreElasticCache.Helpers;
using MvcCoreElasticCache.Models;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace MvcCoreElasticCache.Services
{
    public class ServiceAWSCache
    {
        private IDatabase cache;

        public ServiceAWSCache()
        {
            this.cache = HelperCacheRedis.Connection.GetDatabase();
        }

        public async Task<List<Coche>> GetCochesFavoritosAsync()
        {
            // almacenaremos una colección de coches en formato json
            string json = await this.cache.StringGetAsync("cochesfavoritos");
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
            await this.cache.StringSetAsync("cochesfavoritos", json, TimeSpan.FromMinutes(30));
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
                    await this.cache.KeyDeleteAsync("cochesfavoritos");
                }
                else
                {
                    // almacenamos los coches
                    string json = JsonConvert.SerializeObject(coches);

                    //actualizamos la colección

                    await this.cache.StringSetAsync("cochesfavoritos", json, TimeSpan.FromMinutes(30));
                }
            }
        }
    }
}
