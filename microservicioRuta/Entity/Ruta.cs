using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace microservicioRuta.Entity
{
	public class Ruta
	{
		[BsonId]
		[BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
		public string id { get; set; }

		[BsonElement("Nom")]
		public string nom { get; set; }

		[BsonElement("Descripcio")]
		public string descripcio { get; set; }

		[BsonElement("Link")]
		public string link { get; set; }

		[BsonElement("Actiu")]
		public bool actiu { get; set; }

		[BsonElement("DataCreacio")]
		public DateTime dataCreacio { get; set; }

		[BsonElement("DataModificacio")]
		public DateTime dataModificacio { get; set; }

		[BsonElement("NumConsultes")]
		public int numConsultes { get; set; }

		[BsonElement("idRefugi")]
		public String  idRefugi { get; set; }

		[BsonElement("idCim")]
		public String idCim { get; set; }

		[BsonElement("UrlPic")]
		public String urlPic { get; set; }
	}
}
