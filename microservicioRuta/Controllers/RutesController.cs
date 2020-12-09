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
	[Route("api/[controller]")]
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
		public async Task<ActionResult> PostRuta(RutaPostInput rutaInput)
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
				SendMessageToMicroserveiCims(rutaInput.IdCim);
			} 
			
			if (rutaInput.IdRefugi != null)
			{
				SendMessageToMicroServeiRefugis();
			}

			return Ok(ruta);
		}

		[HttpPut]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status202Accepted)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult> ModifyRuta(Ruta ruta)
		{
			try
			{
				await _repositoryRutes.Update(ruta);
			}
			catch (Exception ex)
			{

				return BadRequest(ex.Message);
			}

			return CreatedAtAction("GetRuta", new { id = ruta.id }, ruta);
		}

		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<Ruta>> GetRuta(string id)
		{
			var ruta = await _repositoryRutes.GetRuta(id);

			ruta.numConsultes = ruta.numConsultes + 1;

			await _repositoryRutes.Update(ruta);

			if (ruta == null)
			{
				return NotFound();
			}
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

		private void SendMessageToMicroserveiCims(string idcim)
		{
			var factory = new ConnectionFactory() { HostName = "localhost" };
			using (var connection = factory.CreateConnection())
			{
				using (var channel = connection.CreateModel())
				{
					String cola = "MicroserveiCims";
					channel.QueueDeclare(cola, false, false, false, null);
					String message = string.Format("{0}", idcim);

					var body = Encoding.UTF8.GetBytes(message);

					channel.BasicPublish("", cola, null, body);
				}
			}
		}

		private void SendMessageToMicroServeiRefugis()
		{
			throw new NotImplementedException();
		}
	}
}
