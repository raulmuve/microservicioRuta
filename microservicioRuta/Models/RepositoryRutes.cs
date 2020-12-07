using microserviceRefugi.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace microservicioRuta.Models
{
	public class RepositoryRutes : IRepositoryRutes
	{
		MongoDBContext db = new MongoDBContext();

		public async Task Add(Ruta ruta)
		{
			try
			{
				await db.Rutes.InsertOneAsync(ruta);
			}
			catch (Exception)
			{

				throw;
			}
		}

		public Task<Ruta> GetRuta(string id)
		{
			try
			{
				FilterDefinition<Ruta> ruta = Builders<Ruta>.Filter.Eq("id", id);
				return db.Rutes.Find(ruta).FirstOrDefaultAsync();
			}
			catch (Exception)
			{

				throw;
			}
		}
	}
}
