using System.Threading.Tasks;

namespace microservicioRuta.Models
{
	public interface IRepositoryRutes
	{
		Task Add(Ruta ruta);

		Task<Ruta> GetRuta(string id);
	}
}
