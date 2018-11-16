using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NHibernate;
using NHibernate.Linq;
using SNACKS.Data;
using SNACKS.Models;

namespace SNACKS.Controllers
{
    [Produces("application/json")]
    [Route("api/IngresosProducto")]
    [ApiController]
    public class IngresosProductoController : UtilController
    {
        public IngresosProductoController(ISessionFactory factory) : base(factory) { }

        [HttpPost("GetIngresosProducto")]
        public async Task<IActionResult> GetIngresosProducto([FromBody] Paginacion paginacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            List<IngresoProducto> lista = null;
            int totalRegistros = 0;

            using (var sn = factory.OpenSession())
            {
                IQueryable<IngresoProducto> query = sn.Query<IngresoProducto>();

                foreach (Filtro filtro in paginacion.Filtros)
                {
                    switch (filtro.K)
                    {
                        case Constantes.Uno:
                            query = query.Where(x => x.FechaCreacion.Date == filtro.D.Date);
                            break;
                    }
                }

                totalRegistros = await query.CountAsync();

                query = query.OrderByDescending(x => x.FechaCreacion);

                AsignarPaginacion(paginacion, ref query);

                lista = await query.ToListAsync();
            }

            return Ok(new
            {
                Lista = lista,
                TotalRegistros = totalRegistros
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetIngresoProducto([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IngresoProducto ingresoProducto = null;

            using (var sn = factory.OpenSession())
            {
                ingresoProducto = await sn.GetAsync<IngresoProducto>(id);
                ingresoProducto.Items = await sn.Query<ItemIngresoProducto>().Where(x => x.IdIngresoProducto == id).ToListAsync();
            }

            if (ingresoProducto == null)
            {
                return NotFound();
            }

            return Ok(ingresoProducto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutIngresoProducto([FromRoute] int id, [FromBody] IngresoProducto ingresoProducto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ingresoProducto.IdIngresoProducto)
            {
                return BadRequest();
            }

            using (var sn = factory.OpenSession())
            {
                using (var tx = sn.BeginTransaction())
                {
                    try
                    {
                        var items = sn.Query<ItemIngresoProducto>()
                            .Where(x => x.IdIngresoProducto == id)
                            .ToList();

                        foreach (var item in items)
                        {
                            InventarioProducto inventario = await sn.Query<InventarioProducto>()
                                .Where(x => x.IdAlmacen == ingresoProducto.Almacen.IdAlmacen
                                    && x.IdProducto == item.Producto.IdProducto)
                                .FirstOrDefaultAsync();

                            if (inventario != null && inventario.Stock >= (item.Cantidad * item.Factor))
                            {
                                inventario.Stock = inventario.Stock - (item.Cantidad * item.Factor);
                            }
                            else
                            {
                                return StatusCode(500, "No hay stock disponible.");
                            }
                        }

                        sn.Delete(string.Format("FROM ItemIngresoProducto WHERE IdIngresoProducto = {0}", id));

                        foreach (var item in ingresoProducto.Items)
                        {
                            InventarioProducto inventario = await sn.Query<InventarioProducto>()
                                .Where(x => x.IdAlmacen == ingresoProducto.Almacen.IdAlmacen
                                    && x.IdProducto == item.Producto.IdProducto)
                                .FirstOrDefaultAsync();

                            if (inventario == null)
                            {
                                sn.Save(new InventarioProducto
                                {
                                    IdAlmacen = ingresoProducto.Almacen.IdAlmacen,
                                    IdProducto = item.Producto.IdProducto,
                                    Stock = (item.Cantidad * item.Factor)
                                });
                            }
                            else
                            {
                                inventario.Stock = inventario.Stock + (item.Cantidad * item.Factor);
                            }

                            item.IdIngresoProducto = id;

                            sn.Save(item);
                        }

                        IngresoProducto ingresoProductoBD = sn.Get<IngresoProducto>(id);

                        ingresoProductoBD.IdLote = ingresoProducto.IdLote;
                        ingresoProductoBD.Usuario = ingresoProductoBD.Usuario;
                        ingresoProductoBD.Comentario = ingresoProducto.Comentario;

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
        public async Task<IActionResult> PostIngresoProducto([FromBody] IngresoProducto ingresoProducto)
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
                        ingresoProducto.FechaCreacion = DateTime.Now;

                        sn.Save(ingresoProducto);

                        foreach (var item in ingresoProducto.Items)
                        {
                            InventarioProducto inventario = await sn.Query<InventarioProducto>()
                                .Where(x => x.IdAlmacen == ingresoProducto.Almacen.IdAlmacen
                                    && x.IdProducto == item.Producto.IdProducto)
                                .FirstOrDefaultAsync();

                            if (inventario == null)
                            {
                                sn.Save(new InventarioProducto
                                {
                                    IdAlmacen = ingresoProducto.Almacen.IdAlmacen,
                                    IdProducto = item.Producto.IdProducto,
                                    Stock = (item.Cantidad * item.Factor)
                                });
                            }
                            else
                            {
                                inventario.Stock = inventario.Stock + (item.Cantidad * item.Factor);
                            }

                            item.IdIngresoProducto = ingresoProducto.IdIngresoProducto;

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
        public async Task<IActionResult> DeleteIngresoProducto([FromRoute] int id)
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
                        IngresoProducto ingresoProducto = sn.Get<IngresoProducto>(id);

                        List<ItemIngresoProducto> items = await sn.Query<ItemIngresoProducto>()
                            .Where(x => x.IdIngresoProducto == id)
                            .ToListAsync();

                        foreach (var item in items)
                        {
                            InventarioProducto inventario = await sn.Query<InventarioProducto>()
                                .Where(x => x.IdAlmacen == ingresoProducto.Almacen.IdAlmacen
                                    && x.IdProducto == item.Producto.IdProducto)
                                .FirstOrDefaultAsync();

                            if (inventario != null && inventario.Stock >= (item.Cantidad * item.Factor))
                            {
                                inventario.Stock = inventario.Stock - (item.Cantidad * item.Factor);
                            }
                            else
                            {
                                throw new Exception("No hay stock disponible.");
                            }
                        }

                        sn.Delete(string.Format("FROM ItemIngresoProducto WHERE IdIngresoProducto = {0}", id));

                        sn.Delete(ingresoProducto);

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