using microservicioRuta.CustomResponse;
using microservicioRuta.Entity;
using microservicioRuta.Models;
using microservicioRuta.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading.Tasks;

namespace microservicioRuta.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class RutesController : ControllerBase
	{
		private readonly IRepositoryRutes _repositoryRutes;

		public RutesController(IRepositoryRutes repositoryRutes)
		{
			_repositoryRutes = repositoryRutes;
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult> Add(RutaPostInput rutaInput)
		{
			Ruta ruta = null;
			try
			{
				ruta = await _repositoryRutes.Add(rutaInput);
			}
			catch (Exception ex)
			{

				return BadRequest(ex.Message);
			}

			if (rutaInput.IdCim != null)
			{
				SendMessageToMicroserveiCims("addRuta",rutaInput.IdCim);
			} 
			
			if (rutaInput.IdRefugi != null)
			{
				SendMessageToMicroServeiRefugis("addRuta", ruta.idRefugi, ruta.id) ;
			}

			return Ok(ruta);
		}

		[HttpPost("modify")]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status202Accepted)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult> Modify(Ruta rutaInput)
		{
			Ruta ruta = null;
			try
			{
				ruta = await _repositoryRutes.Update(rutaInput);
			}
			catch (Exception ex)
			{

				return BadRequest(ex.Message);
			}

			if (rutaInput.idCim != null)
			{
				SendMessageToMicroserveiCims("addRuta", rutaInput.idCim);
			}

			if (rutaInput.idRefugi != null)
			{
				SendMessageToMicroServeiRefugis("addRuta", ruta.idRefugi, ruta.id);
			}

			return CreatedAtAction("Search", new { id = ruta.id }, ruta);
		}

		[HttpPost("delete/{id}")]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status202Accepted)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult> Delete(String id)
		{
			try
			{
				var rutaInput = await _repositoryRutes.GetRuta(id);

				if (rutaInput == null)
				{
					RutaNotFound rnf = new RutaNotFound(id, "Ruta -> Ruta not found");

					return NotFound(rnf);
				}

				rutaInput = await _repositoryRutes.Delete(rutaInput);

				if (rutaInput.idCim != null)
				{
					SendMessageToMicroserveiCims("deleteRuta", rutaInput.idCim);
				}

				if (rutaInput.idRefugi != null)
				{
					SendMessageToMicroServeiRefugis("deleteRuta", rutaInput.idRefugi, id);
				}

				return CreatedAtAction("Search", new { id = id }, rutaInput);
			}
			catch (Exception ex)
			{

				return BadRequest(ex.Message);
			}
			
		}

		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<Ruta>> Search(string id)
		{
			var ruta = await _repositoryRutes.GetRuta(id);

			if (ruta == null)
			{
				RutaNotFound rnf = new RutaNotFound(id, "Ruta -> Ruta not found");

				return NotFound(rnf);
			}

			ruta.numConsultes = ruta.numConsultes + 1;

			await _repositoryRutes.Update(ruta);

			return Ok(ruta);
		}

		[HttpGet("top10")]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<Ruta>> Top10()
		{
			var ruta = await _repositoryRutes.TopTen();

			if (ruta == null)
			{
				return NotFound();
			}
			return Ok(ruta);
		}

		[HttpGet("bycim/{id}")]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<Ruta>> SearchByCim(string id)
		{
			var ruta = await _repositoryRutes.SearchByCim(id);

			if (ruta == null)
			{
				return NotFound();
			}
			return Ok(ruta);
		}

		[HttpGet("byrefugi/{id}")]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<Ruta>> SearchByRefugi(string id)
		{
			var ruta = await _repositoryRutes.SearchByRefugi(id);

			if (ruta == null)
			{
				return NotFound();
			}
			return Ok(ruta);
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<Ruta>> SearchAll(string id)
		{
			var ruta = await _repositoryRutes.SearchAll();

			if (ruta == null)
			{
				return NotFound();
			}
			return Ok(ruta);
		}



		private void SendMessageToMicroserveiCims(string Operacio, string idcim)
		{
			var factory = new ConnectionFactory() { HostName = "localhost" };
			using (var connection = factory.CreateConnection())
			{
				using (var channel = connection.CreateModel())
				{
					String cola = "MicroserveiCims";
					channel.QueueDeclare(cola, false, false, false, null);
					String message = string.Format("{0};{1}", Operacio, idcim);

					var body = Encoding.UTF8.GetBytes(message);

					channel.BasicPublish("", cola, null, body);
				}
			}
		}

		private void SendMessageToMicroServeiRefugis(String Operacio, String idRefugi, String idRuta)
		{
			var factory = new ConnectionFactory() { HostName = "localhost" };
			using (var connection = factory.CreateConnection())
			{
				using (var channel = connection.CreateModel())
				{
					String cola = "MicroserveiRefugis";
					channel.QueueDeclare(cola, false, false, false, null);
					String message = string.Format("{0};{1};{2}", Operacio, idRefugi, idRuta);

					var body = Encoding.UTF8.GetBytes(message);

					channel.BasicPublish("", cola, null, body);
				}
			}
		}
	}
}
