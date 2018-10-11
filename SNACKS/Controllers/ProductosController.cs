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
    [Route("api/Productos")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        public IRepositorioBase<Producto> Repositorio { get; }

        public ProductosController(IRepositorioBase<Producto> repositorio)
        {
            Repositorio = repositorio;
        }

        [HttpPost("GetProductos")]
        public async Task<IActionResult> GetProductos([FromBody] Paginacion paginacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var filtros = new List<Expression<Func<Producto, bool>>>();

            foreach (Filtro filtro in paginacion.Filtros)
            {
                switch (filtro.K)
                {
                    case Constantes.Uno:
                        filtros.Add(x => x.Nombre.Contains(filtro.V));
                        break;
                    case Constantes.Dos:
                        filtros.Add(x => x.EsInsumo == filtro.B);
                        break;
                }
            }

            var result = await Repositorio.ObtenerTodosAsync(paginacion, filtros);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProducto([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var producto = await Repositorio.ObtenerAsync(id);

            if (producto == null)
            {
                return NotFound();
            }

            return Ok(producto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducto([FromRoute] int id, [FromBody] Producto producto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != producto.IdProducto)
            {
                return BadRequest();
            }

            try
            {
                await Repositorio.ActualizarAsync(producto);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

            return Ok(true);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Producto producto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await Repositorio.RegistrarAsync(producto);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

            return Ok(true);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var producto = await Repositorio.ObtenerAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            try
            {
                await Repositorio.EliminarAsync(producto);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

            return Ok(true);
        }
    }
}