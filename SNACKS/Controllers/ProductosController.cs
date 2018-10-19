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
        public IRepositorioBase<ItemProducto> RepositorioItem { get; }

        public ProductosController(IRepositorioBase<Producto> repositorio, 
            IRepositorioBase<ItemProducto> repositorioItem)
        {
            Repositorio = repositorio;
            RepositorioItem = repositorioItem;
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
                    case Constantes.Tres:
                        filtros.Add(x => x.Categoria.IdCategoria == filtro.N);
                        break;
                }
            }

            var result = await Repositorio.ObtenerTodosAsync(paginacion, filtros, new string[] { Constantes.Categoria });

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProducto([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var producto = await Repositorio.ObtenerAsync(id, new string[] { Constantes.Categoria, Constantes.Items + '.' + Constantes.Unidad });

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
                await Repositorio.ActualizarAsync(producto, new object[] { producto.Categoria });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
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
                List<object> referencias = new List<object>();
                referencias.Add(producto.Categoria);
                foreach (var item in producto.Items)
                {
                    referencias.Add(item.Unidad);
                }
                await Repositorio.RegistrarAsync(producto, referencias.ToArray());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
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

            var producto = await Repositorio.ObtenerAsync(id, new string[] { Constantes.Items });
            if (producto == null)
            {
                return NotFound();
            }

            try
            {
                await RepositorioItem.EliminarAsync(producto.Items.ToArray());
                await Repositorio.EliminarAsync(new Producto[] { producto });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(true);
        }

        [HttpPost("AddItem")]
        public async Task<IActionResult> PostItem([FromBody] ItemProducto item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await RepositorioItem.RegistrarAsync(item, new object[] { item.Producto, item.Unidad });
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
                await RepositorioItem.EliminarAsync(new ItemProducto[] { item });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(true);
        }
    }
}