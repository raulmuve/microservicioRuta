using microservicioRuta.Entity;
using microservicioRuta.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace microservicioRuta.Repository
{
	public interface IRepositoryRutes
	{
		Task<Ruta> Add(RutaPostInput ruta);

		Task<Ruta> GetRuta(string id);

		Task Update(Ruta ruta);

		Task<List<Ruta>> TopTen();
	}
}
