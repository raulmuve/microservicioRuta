using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using microservicioRuta.Repository;
using microservicioRuta.Entity;

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
		public async Task<ActionResult> PostCim(Ruta ruta)
		{
			try
			{
				await _repositoryRutes.Add(ruta);
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

		[HttpGet("top")]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<Ruta>> Top10()
		{
			var ruta = await _repositoryRutes.Top10();

			if (ruta == null)
			{
				return NotFound();
			}
			return Ok(ruta);
		}
	}
}
