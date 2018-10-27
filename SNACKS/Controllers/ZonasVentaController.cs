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
    [Route("api/ZonasVenta")]
    [ApiController]
    public class ZonasVentaController : ControllerBase
    {
        public IRepositorioBase<ZonaVenta> Repositorio { get; }

        public ZonasVentaController(IRepositorioBase<ZonaVenta> repositorio)
        {
            Repositorio = repositorio;
        }

        [HttpPost("GetZonasVenta")]
        public async Task<IActionResult> GetZonasVenta([FromBody] Paginacion paginacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var filtros = new List<Expression<Func<ZonaVenta, bool>>>();

            foreach (Filtro filtro in paginacion.Filtros)
            {
                switch (filtro.K)
                {
                    case Constantes.Uno:
                        filtros.Add(x => x.Nombre.Contains(filtro.V));
                        break;
                }
            }

            var result = await Repositorio.ObtenerTodosAsync(paginacion, filtros);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetZonaVenta([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var zonaVenta = await Repositorio.ObtenerAsync(id);

            if (zonaVenta == null)
            {
                return NotFound();
            }

            return Ok(zonaVenta);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutZonaVenta([FromRoute] int id, [FromBody] ZonaVenta zonaVenta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != zonaVenta.IdZonaVenta)
            {
                return BadRequest();
            }

            try
            {
                await Repositorio.ActualizarAsync(zonaVenta);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(true);
        }

        [HttpPost]
        public async Task<IActionResult> PostZonaVenta([FromBody] ZonaVenta zonaVenta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await Repositorio.RegistrarAsync(zonaVenta);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(true);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteZonaVenta([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var zonaVenta = await Repositorio.ObtenerAsync(id);
            if (zonaVenta == null)
            {
                return NotFound();
            }

            try
            {
                await Repositorio.EliminarAsync(new ZonaVenta[] { zonaVenta });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(true);
        }
    }
}