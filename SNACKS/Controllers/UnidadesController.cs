using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SNACKS.Data;
using SNACKS.Models;

namespace SNACKS.Controllers
{
    [Produces("application/json")]
    [Route("api/Unidades")]
    [ApiController]
    public class UnidadesController : ControllerBase
    {
        public IRepositorioBase<Unidad> Repositorio { get; }

        public UnidadesController(IRepositorioBase<Unidad> repositorio)
        {
            Repositorio = repositorio;
        }

        [HttpPost("GetUnidades")]
        public async Task<IActionResult> GetUnidades([FromBody] Paginacion paginacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var filtros = new List<Expression<Func<Unidad, bool>>>();

            foreach (Filtro filtro in paginacion.Filtros)
            {
                switch (filtro.K)
                {
                    case Constantes.Uno:
                        filtros.Add(x => x.Nombre.Contains(filtro.V));
                        break;
                    case Constantes.Dos:
                        filtros.Add(x => x.Abreviacion.Contains(filtro.V));
                        break;
                }
            }

            var result = await Repositorio.ObtenerTodosAsync(paginacion, filtros);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUnidad([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var unidad = await Repositorio.ObtenerAsync(id);

            if (unidad == null)
            {
                return NotFound();
            }

            return Ok(unidad);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUnidad([FromRoute] int id, [FromBody] Unidad unidad)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != unidad.IdUnidad)
            {
                return BadRequest();
            }

            try
            {
                await Repositorio.ActualizarAsync(unidad);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(true);
        }

        [HttpPost]
        public async Task<IActionResult> PostUnidad([FromBody] Unidad unidad)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await Repositorio.RegistrarAsync(unidad);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(true);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUnidad([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var unidad = await Repositorio.ObtenerAsync(id);
            if (unidad == null)
            {
                return NotFound();
            }

            try
            {
                await Repositorio.EliminarAsync(new Unidad[] { unidad });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(true);
        }
    }
}