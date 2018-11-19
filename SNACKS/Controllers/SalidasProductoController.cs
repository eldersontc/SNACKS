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
    [Route("api/SalidasProducto")]
    [ApiController]
    public class SalidasProductoController : UtilController
    {
        public SalidasProductoController(ISessionFactory factory) : base(factory) { }

        [HttpPost("GetSalidasProducto")]
        public async Task<IActionResult> GetSalidasProducto([FromBody] Paginacion paginacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            List<SalidaProducto> lista = null;
            int totalRegistros = 0;

            using (var sn = factory.OpenSession())
            {
                IQueryable<SalidaProducto> query = sn.Query<SalidaProducto>();

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
        public async Task<IActionResult> GetSalidaProducto([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            SalidaProducto salidaProducto = null;

            using (var sn = factory.OpenSession())
            {
                salidaProducto = await sn.GetAsync<SalidaProducto>(id);
                salidaProducto.Items = await sn.Query<ItemSalidaProducto>().Where(x => x.IdSalidaProducto == id).ToListAsync();
            }

            if (salidaProducto == null)
            {
                return NotFound();
            }

            return Ok(salidaProducto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutSalidaProducto([FromRoute] int id, [FromBody] SalidaProducto salidaProducto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != salidaProducto.IdSalidaProducto)
            {
                return BadRequest();
            }

            using (var sn = factory.OpenSession())
            {
                using (var tx = sn.BeginTransaction())
                {
                    try
                    {
                        List<ItemSalidaProducto> items = await sn.Query<ItemSalidaProducto>()
                            .Where(x => x.IdSalidaProducto == id)
                            .ToListAsync();

                        foreach (var item in items)
                        {
                            InventarioProducto inventario = await sn.Query<InventarioProducto>()
                                .Where(x => x.IdAlmacen == salidaProducto.Almacen.IdAlmacen
                                    && x.IdProducto == item.Producto.IdProducto)
                                .FirstOrDefaultAsync();

                            inventario.Stock = inventario.Stock + (item.Cantidad * item.Factor);
                        }

                        sn.Delete(string.Format("FROM ItemSalidaProducto WHERE IdSalidaProducto = {0}", id));

                        foreach (var item in salidaProducto.Items)
                        {
                            InventarioProducto inventario = await sn.Query<InventarioProducto>()
                                .Where(x => x.IdAlmacen == salidaProducto.Almacen.IdAlmacen
                                    && x.IdProducto == item.Producto.IdProducto)
                                .FirstOrDefaultAsync();

                            if (inventario != null && inventario.Stock >= (item.Cantidad * item.Factor))
                            {
                                inventario.Stock = inventario.Stock - (item.Cantidad * item.Factor);
                            }
                            else
                            {
                                throw new Exception("Sin stock para el producto: " + item.Producto.Nombre);
                            }

                            item.IdSalidaProducto = salidaProducto.IdSalidaProducto;

                            sn.Save(item);
                        }

                        SalidaProducto salidaProductoBD = sn.Get<SalidaProducto>(id);

                        salidaProductoBD.Comentario = salidaProducto.Comentario;

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
        public async Task<IActionResult> PostSalidaProducto([FromBody] SalidaProducto salidaProducto)
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
                        salidaProducto.FechaCreacion = DateTime.Now;

                        sn.Save(salidaProducto);

                        foreach (var item in salidaProducto.Items)
                        {
                            InventarioProducto inventario = await sn.Query<InventarioProducto>()
                                .Where(x => x.IdAlmacen == salidaProducto.Almacen.IdAlmacen
                                    && x.IdProducto == item.Producto.IdProducto)
                                .FirstOrDefaultAsync();

                            if (inventario != null && inventario.Stock >= (item.Cantidad * item.Factor))
                            {
                                inventario.Stock = inventario.Stock - (item.Cantidad * item.Factor);
                            }
                            else
                            {
                                throw new Exception("Sin stock para el producto: " + item.Producto.Nombre);
                            }

                            item.IdSalidaProducto = salidaProducto.IdSalidaProducto;

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
        public async Task<IActionResult> DeleteSalidaProducto([FromRoute] int id)
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
                        SalidaProducto salidaProducto = sn.Get<SalidaProducto>(id);

                        List<ItemSalidaProducto> items = await sn.Query<ItemSalidaProducto>()
                            .Where(x => x.IdSalidaProducto == id)
                            .ToListAsync();

                        foreach (var item in items)
                        {
                            InventarioProducto inventario = await sn.Query<InventarioProducto>()
                                .Where(x => x.IdAlmacen == salidaProducto.Almacen.IdAlmacen
                                    && x.IdProducto == item.Producto.IdProducto)
                                .FirstOrDefaultAsync();

                            inventario.Stock = inventario.Stock + (item.Cantidad * item.Factor);
                        }

                        sn.Delete(string.Format("FROM ItemSalidaProducto WHERE IdSalidaProducto = {0}", id));

                        if (salidaProducto.IdPedido != null)
                        {
                            Pedido pedido = sn.Get<Pedido>(salidaProducto.IdPedido);

                            pedido.FechaEntrega = null;

                            if (pedido.Pago == 0)
                            {
                                pedido.Estado = Constantes.Pendiente;
                            }
                            else if (pedido.Pago == pedido.Total)
                            {
                                pedido.Estado = Constantes.Pagado;
                            }
                            else if (pedido.Pago < pedido.Total)
                            {
                                pedido.Estado = Constantes.PagoParcial;
                            }
                        }

                        sn.Delete(salidaProducto);

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