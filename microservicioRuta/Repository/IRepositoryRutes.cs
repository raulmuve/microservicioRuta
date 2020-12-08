using System.Collections.Generic;
using System.Threading.Tasks;
using microservicioRuta.Entity;

namespace microservicioRuta.Repository
{
	public interface IRepositoryRutes
	{
		Task Add(Ruta ruta);

		Task<Ruta> GetRuta(string id);

		Task Update(Ruta ruta);

		Task<List<Ruta>> Top10();
	}
}
