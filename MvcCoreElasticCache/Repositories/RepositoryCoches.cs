using MvcCoreElasticCache.Models;
using System.Xml.Linq;

namespace MvcCoreElasticCache.Repositories
{
    public class RepositoryCoches
    {
        private XDocument document;

        public RepositoryCoches()
        {
            string ruta = "MvcCoreElasticCache.Documents.coches.xml";
            Stream st = this.GetType().Assembly.GetManifestResourceStream(ruta);
            this.document = XDocument.Load(st);
        }

        public List<Coche> GetCoches()
        {
            var consulta = from datos in this.document.Descendants("coche")
                           select new Coche
                           {
                               IdCoche = int.Parse(datos.Element("idcoche").Value),
                               Marca = datos.Element("marca").Value,
                               Modelo = datos.Element("modelo").Value,
                               Imagen = datos.Element("imagen").Value
                           };

            return consulta.ToList();
        }

        public Coche GetCoche(int id)
        {
            return GetCoches().FirstOrDefault(x => x.IdCoche == id);
        }
    }
}
