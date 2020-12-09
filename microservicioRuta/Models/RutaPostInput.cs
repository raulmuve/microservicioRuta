using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace microservicioRuta.Models
{
	
	public class RutaPostInput
	{
		[Required(AllowEmptyStrings = false)]
		public string Nom { get; set; }

		[Required(AllowEmptyStrings = false)]
		public string Descripcio { get; set; }

		[Required(AllowEmptyStrings = false)]
		public string Link { get; set; }


		public string IdRefugi { get; set; }

		public string IdCim { get; set; }

		[Required(AllowEmptyStrings = false)]
		public string UrlPic { get; set; }
	}
}
