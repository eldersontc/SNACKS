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
    [Route("api/SalidasInsumo")]
    [ApiController]
    public class SalidasInsumoController : ControllerBase
    {
        public IRepositorioBase<SalidaInsumo> Repositorio { get; }
        public IRepositorioBase<ItemSalidaInsumo> RepositorioItem { get; }

        public SalidasInsumoController(IRepositorioBase<SalidaInsumo> repositorio,
            IRepositorioBase<ItemSalidaInsumo> repositorioItem)
        {
            Repositorio = repositorio;
            RepositorioItem = repositorioItem;
        }

        [HttpPost("GetSalidasInsumo")]
        public async Task<IActionResult> GetSalidasInsumo([FromBody] Paginacion paginacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var filtros = new List<Expression<Func<SalidaInsumo, bool>>>();

            foreach (Filtro filtro in paginacion.Filtros)
            {
                switch (filtro.K)
                {
                    case Constantes.Uno:
                        filtros.Add(x => x.FechaCreacion.Date == filtro.D.Date);
                        break;
                }
            }

            var result = await Repositorio.ObtenerTodosAsync(paginacion, filtros, new string[] { Constantes.Usuario });

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSalidaInsumo([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ingresoInsumo = await Repositorio.ObtenerAsync(id, new string[] {
                Constantes.Usuario,
                Constantes.Items + '.' + Constantes.Producto,
                Constantes.Items + '.' + Constantes.Unidad
            });

            if (ingresoInsumo == null)
            {
                return NotFound();
            }

            return Ok(ingresoInsumo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutSalidaInsumo([FromRoute] int id, [FromBody] SalidaInsumo ingresoInsumo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ingresoInsumo.IdSalidaInsumo)
            {
                return BadRequest();
            }

            try
            {
                await Repositorio.ActualizarAsync(ingresoInsumo, new object[] { ingresoInsumo.Usuario });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(true);
        }

        [HttpPost]
        public async Task<IActionResult> PostSalidaInsumo([FromBody] SalidaInsumo ingresoInsumo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                List<object> referencias = new List<object>();
                referencias.Add(ingresoInsumo.Usuario);
                referencias.AddRange(ingresoInsumo.Items
                    .Select(x => x.Producto)
                    .GroupBy(g => g.IdProducto)
                    .Select(g => g.First()).ToList());
                referencias.AddRange(ingresoInsumo.Items
                    .Select(x => x.Unidad)
                    .GroupBy(g => g.IdUnidad)
                    .Select(g => g.First()).ToList());
                ingresoInsumo.FechaCreacion = DateTime.Now;
                await Repositorio.RegistrarAsync(ingresoInsumo, referencias.ToArray());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(true);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSalidaInsumo([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ingresoInsumo = await Repositorio.ObtenerAsync(id, new string[] { Constantes.Items });
            if (ingresoInsumo == null)
            {
                return NotFound();
            }

            try
            {
                await RepositorioItem.EliminarAsync(ingresoInsumo.Items.ToArray());
                await Repositorio.EliminarAsync(new SalidaInsumo[] { ingresoInsumo });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(true);
        }

        [HttpPost("AddItem")]
        public async Task<IActionResult> PostItem([FromBody] ItemSalidaInsumo item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await RepositorioItem.RegistrarAsync(item, new object[] { item.SalidaInsumo, item.Producto, item.Unidad });
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
                await RepositorioItem.EliminarAsync(new ItemSalidaInsumo[] { item });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(true);
        }
    }
}