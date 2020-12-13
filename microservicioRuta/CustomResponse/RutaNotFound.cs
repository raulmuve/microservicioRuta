using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace microservicioRuta.CustomResponse
{
	public class RutaNotFound
	{
		public string id { get; set; }

		public string message { get; set; }

		public RutaNotFound(string _id, string message)
		{
			this.id = _id;
			this.message = message;
		}
	}
}
