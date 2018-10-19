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
    [Route("api/SalidasProducto")]
    [ApiController]
    public class SalidasProductoController : ControllerBase
    {
        public IRepositorioBase<SalidaProducto> Repositorio { get; }
        public IRepositorioBase<ItemSalidaProducto> RepositorioItem { get; }

        public SalidasProductoController(IRepositorioBase<SalidaProducto> repositorio,
            IRepositorioBase<ItemSalidaProducto> repositorioItem)
        {
            Repositorio = repositorio;
            RepositorioItem = repositorioItem;
        }

        [HttpPost("GetSalidasProducto")]
        public async Task<IActionResult> GetSalidasProducto([FromBody] Paginacion paginacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var filtros = new List<Expression<Func<SalidaProducto, bool>>>();

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
        public async Task<IActionResult> GetSalidaProducto([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ingresoProducto = await Repositorio.ObtenerAsync(id, new string[] {
                Constantes.Usuario,
                Constantes.Items + '.' + Constantes.Producto,
                Constantes.Items + '.' + Constantes.Unidad
            });

            if (ingresoProducto == null)
            {
                return NotFound();
            }

            return Ok(ingresoProducto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutSalidaProducto([FromRoute] int id, [FromBody] SalidaProducto ingresoProducto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ingresoProducto.IdSalidaProducto)
            {
                return BadRequest();
            }

            try
            {
                await Repositorio.ActualizarAsync(ingresoProducto, new object[] { ingresoProducto.Usuario });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(true);
        }

        [HttpPost]
        public async Task<IActionResult> PostSalidaProducto([FromBody] SalidaProducto ingresoProducto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                List<object> referencias = new List<object>();
                referencias.Add(ingresoProducto.Usuario);
                referencias.AddRange(ingresoProducto.Items
                    .Select(x => x.Producto)
                    .GroupBy(g => g.IdProducto)
                    .Select(g => g.First()).ToList());
                referencias.AddRange(ingresoProducto.Items
                    .Select(x => x.Unidad)
                    .GroupBy(g => g.IdUnidad)
                    .Select(g => g.First()).ToList());
                ingresoProducto.FechaCreacion = DateTime.Now;
                await Repositorio.RegistrarAsync(ingresoProducto, referencias.ToArray());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(true);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSalidaProducto([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ingresoProducto = await Repositorio.ObtenerAsync(id, new string[] { Constantes.Items });
            if (ingresoProducto == null)
            {
                return NotFound();
            }

            try
            {
                await RepositorioItem.EliminarAsync(ingresoProducto.Items.ToArray());
                await Repositorio.EliminarAsync(new SalidaProducto[] { ingresoProducto });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(true);
        }

        [HttpPost("AddItem")]
        public async Task<IActionResult> PostItem([FromBody] ItemSalidaProducto item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await RepositorioItem.RegistrarAsync(item, new object[] { item.SalidaProducto, item.Producto, item.Unidad });
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
                await RepositorioItem.EliminarAsync(new ItemSalidaProducto[] { item });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(true);
        }
    }
}