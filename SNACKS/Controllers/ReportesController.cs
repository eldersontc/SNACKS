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
    [Route("api/Reportes")]
    [ApiController]
    public class ReportesController : ControllerBase
    {
        public IRepositorioBase<Reporte> Repositorio { get; }
        public IRepositorioBase<ItemReporte> RepositorioItem { get; }

        public ReportesController(IRepositorioBase<Reporte> repositorio,
            IRepositorioBase<ItemReporte> repositorioItem)
        {
            Repositorio = repositorio;
            RepositorioItem = repositorioItem;
        }

        [HttpPost("GetReportes")]
        public async Task<IActionResult> GetReportes([FromBody] Paginacion paginacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var filtros = new List<Expression<Func<Reporte, bool>>>();

            foreach (Filtro filtro in paginacion.Filtros)
            {
                switch (filtro.K)
                {
                    case Constantes.Uno:
                        filtros.Add(x => x.Titulo.Contains(filtro.V));
                        break;
                }
            }

            var result = await Repositorio.ObtenerTodosAsync(paginacion, filtros);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReporte([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reporte = await Repositorio.ObtenerAsync(id, new string[] { Constantes.Items });

            if (reporte == null)
            {
                return NotFound();
            }

            return Ok(reporte);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutReporte([FromRoute] int id, [FromBody] Reporte reporte)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != reporte.IdReporte)
            {
                return BadRequest();
            }

            try
            {
                await Repositorio.ActualizarAsync(reporte);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(true);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Reporte reporte)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await Repositorio.RegistrarAsync(reporte);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(true);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReporte([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reporte = await Repositorio.ObtenerAsync(id, new string[] { Constantes.Items });
            if (reporte == null)
            {
                return NotFound();
            }

            try
            {
                await RepositorioItem.EliminarAsync(reporte.Items.ToArray());
                await Repositorio.EliminarAsync(new Reporte[] { reporte });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(true);
        }

        [HttpPost("AddItem")]
        public async Task<IActionResult> PostItem([FromBody] ItemReporte item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await RepositorioItem.RegistrarAsync(item, new object[] { item.Reporte });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(true);
        }

        [HttpDelete("DeleteItem/{id}")]
        public async Task<IActionResult> DeleteItem([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var item = await RepositorioItem.ObtenerAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            try
            {
                await RepositorioItem.EliminarAsync(new ItemReporte[] { item });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(true);
        }

        [HttpPost("RunReporte")]
        public async Task<IActionResult> RunReporte([FromBody] Reporte reporte)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            List<object> valores = new List<object>() { reporte.Flag };
            foreach (var item in reporte.Items)
            {
                valores.Add(item.Valor);
            }

            try
            {
                var result = await Repositorio.ObtenerEstadisticasAsync(valores.ToArray());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}