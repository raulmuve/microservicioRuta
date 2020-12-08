using MongoDB.Driver;
using System;
using System.Threading.Tasks;
using microservicioRuta.Entity;
using System.Collections.Generic;
using MongoDB.Bson;

namespace microservicioRuta.Repository
{
	public class RepositoryRutes : IRepositoryRutes
	{
		MongoDBContext db = new MongoDBContext();

		public async Task Add(Ruta ruta)
		{
			ruta.dataCreacio = DateTime.Now;
			ruta.dataModificacio = DateTime.Now;
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

		public async Task<List<Ruta>> Top10()
		{
			try
			{
				var sortDefinition = Builders<Ruta>.Sort.Descending(a => a.numConsultes);
				var listOfMovies = db.Rutes.Find(_ => true).Sort(sortDefinition).Limit(10).ToList();

				return listOfMovies;

			}
			catch (Exception)
			{

				throw;
			}
		}

		public async Task Update(Ruta ruta)
		{
			try
			{
				await db.Rutes.ReplaceOneAsync(filter: g => g.id == ruta.id, replacement: ruta);
			}
			catch (Exception)
			{

				throw;
			}
			
		}
	}
}
