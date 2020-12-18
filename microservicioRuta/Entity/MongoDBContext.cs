using MongoDB.Driver;

namespace microservicioRuta.Entity
{
	public class MongoDBContext
	{
		//Definicio constants
		private const string ConnectionString = "mongodb://localhost:27017/";
		private const string Database = "serveiRutes";
		private const string ColeccioRutes = "rutes";

		private readonly IMongoDatabase _mongoDB;

		public MongoDBContext()
		{
			var client = new MongoClient(ConnectionString);
			_mongoDB = client.GetDatabase(Database);
		}

		public IMongoCollection<Ruta> Rutes
		{
			get
			{
				return _mongoDB.GetCollection<Ruta>(ColeccioRutes);
			}
		}
	}
}
