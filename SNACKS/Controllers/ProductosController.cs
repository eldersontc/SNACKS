using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NHibernate;
using NHibernate.Linq;
using SNACKS.Models;

namespace SNACKS.Controllers
{
    [Produces("application/json")]
    [Route("api/Productos")]
    [ApiController]
    public class ProductosController : UtilController
    {
        public ProductosController(ISessionFactory factory) : base(factory) { }

        [HttpPost("GetProductos")]
        public async Task<IActionResult> GetProductos([FromBody] Paginacion paginacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            List<Producto> lista = null;
            int totalRegistros = 0;

            using (var sn = factory.OpenSession())
            {
                IQueryable<Producto> query = sn.Query<Producto>();

                foreach (Filtro filtro in paginacion.Filtros)
                {
                    switch (filtro.K)
                    {
                        case Constantes.Uno:
                            query = query.Where(x => x.Nombre.Contains(filtro.V));
                            break;
                        case Constantes.Dos:
                            query = query.Where(x => x.EsInsumo == filtro.B);
                            break;
                        case Constantes.Tres:
                            query = query.Where(x => x.Categoria.IdCategoria == filtro.N);
                            break;
                    }
                }

                totalRegistros = await query.CountAsync();

                AsignarPaginacion(paginacion, ref query);

                query = query.OrderBy(x => x.Nombre);

                lista = await query.ToListAsync();
            }

            return Ok(new {
                Lista = lista,
                TotalRegistros = totalRegistros
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProducto([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Producto producto = null;

            using (var sn = factory.OpenSession())
            {
                producto = await sn.GetAsync<Producto>(id);
                producto.Items = await sn.Query<ItemProducto>().Where(x => x.IdProducto == id).ToListAsync();
            }

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

            using (var sn = factory.OpenSession())
            {
                using (var tx = sn.BeginTransaction())
                {
                    try
                    {
                        sn.Delete(string.Format("FROM ItemProducto WHERE IdProducto = {0}", id));

                        foreach (var item in producto.Items)
                        {
                            item.IdProducto = id;
                            sn.Save(item);
                        }

                        sn.SaveOrUpdate(producto);

                        await tx.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await tx.RollbackAsync();
                        return StatusCode(500, ex.Message);
                    }
                }
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

            using (var sn = factory.OpenSession())
            {
                using (var tx = sn.BeginTransaction())
                {
                    try
                    {
                        sn.Save(producto);

                        foreach (var item in producto.Items)
                        {
                            item.IdProducto = producto.IdProducto;
                            sn.Save(item);
                        }

                        await tx.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await tx.RollbackAsync();
                        return StatusCode(500, ex.Message);
                    }
                }
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

            using (var sn = factory.OpenSession())
            {
                using (var tx = sn.BeginTransaction())
                {
                    try
                    {
                        sn.Delete(string.Format("FROM ItemProducto WHERE IdProducto = {0}", id));

                        sn.Delete(new Producto { IdProducto = id });

                        await tx.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await tx.RollbackAsync();
                        return StatusCode(500, ex.Message);
                    }
                }
            }

            return Ok(true);
        }
    }
}