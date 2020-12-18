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

		Task<Ruta> Update(Ruta ruta);

		Task<Ruta> Delete(Ruta ruta);

		Task<List<Ruta>> TopTen();

		Task<List<Ruta>> SearchAll();

		Task<List<Ruta>> SearchByCim(string idCim);

		Task<List<Ruta>> SearchByRefugi(string idRefugi);
	}	
}
