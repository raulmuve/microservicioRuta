using MongoDB.Driver;
using System;
using System.Threading.Tasks;
using microservicioRuta.Entity;
using System.Collections.Generic;
using microservicioRuta.Models;

namespace microservicioRuta.Repository
{
	public class RepositoryRutes : IRepositoryRutes
	{
		readonly MongoDBContext db = new MongoDBContext();

		public async Task<Ruta> Add(RutaPostInput rutaInput)
		{
			Ruta ruta = new Ruta();
			ruta.nom = rutaInput.Nom;
			ruta.descripcio = rutaInput.Descripcio;
			ruta.link = rutaInput.Link;
			ruta.idCim = rutaInput.IdCim;
			ruta.idRefugi = rutaInput.IdRefugi;
			ruta.urlPic = rutaInput.UrlPic;
			ruta.dataCreacio = DateTime.Now;
			ruta.dataModificacio = DateTime.Now;
			ruta.actiu = true;
			ruta.numConsultes = 0;
			try
			{
				await db.Rutes.InsertOneAsync(ruta);
			}
			catch (Exception)
			{

				throw;
			}

			return LastDocumentInserted();
		}


		public async Task<Ruta> Update(Ruta rutaInput)
		{
			try
			{
				if (rutaInput.id == null) return null;

				rutaInput.dataModificacio = DateTime.Now;
				rutaInput.actiu = true;
				await db.Rutes.ReplaceOneAsync(filter: g => g.id == rutaInput.id, replacement: rutaInput);
			}
			catch (Exception)
			{

				throw;
			}

			return rutaInput;
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

		public async Task<List<Ruta>> TopTen()
		{
			try
			{
				var sortDefinition = Builders<Ruta>.Sort.Descending(a => a.numConsultes);
				return await db.Rutes.Find(g => g.actiu == true).Sort(sortDefinition).Limit(10).ToListAsync();

			}
			catch (Exception)
			{

				throw;
			}
		}


		public Ruta LastDocumentInserted()
		{
			var sortDefinition = Builders<Ruta>.Sort.Descending(a => a.id);
			return db.Rutes.Find(_ => true).Sort(sortDefinition).Limit(1).FirstOrDefault();
		}

		public async Task<Ruta> Delete(Ruta rutaInput)
		{
			try
			{
				rutaInput.dataModificacio = DateTime.Now;
				rutaInput.actiu = false;
				await db.Rutes.ReplaceOneAsync(filter: g => g.id == rutaInput.id, replacement: rutaInput);
			}
			catch (Exception)
			{

				throw;
			}

			return rutaInput;
		}

		public async Task<List<Ruta>> SearchAll()
		{
			try
			{
				var sortDefinition = Builders<Ruta>.Sort.Ascending(a => a.nom);
				return await db.Rutes.Find(a => a.actiu == true).Sort(sortDefinition).ToListAsync();

			}
			catch (Exception)
			{

				throw;
			}
		}

		public async Task<List<Ruta>> SearchByCim(string idCim)
		{
			try
			{
				var filter = Builders<Ruta>.Filter.ElemMatch(x => x.idCim, x => x.Equals(idCim));

				return await db.Rutes.Find(filter).ToListAsync();
			}
			catch (Exception)
			{

				throw;
			}
		}

		public async Task<List<Ruta>> SearchByRefugi(string idRefugi)
		{
			try
			{
				var filter = Builders<Ruta>.Filter.ElemMatch(x => x.idRefugi, x => x.Equals(idRefugi));

				return await db.Rutes.Find(filter).ToListAsync();
			}
			catch (Exception)
			{

				throw;
			}
		}
	}
}
