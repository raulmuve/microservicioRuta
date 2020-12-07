using microservicioRuta.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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

			if (ruta == null)
			{
				return NotFound();
			}
			return Ok(ruta);
		}
	}
}
