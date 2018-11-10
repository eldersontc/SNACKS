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
    [Route("api/IngresosInsumo")]
    [ApiController]
    public class IngresosInsumoController : UtilController
    {
        public IngresosInsumoController(ISessionFactory factory) : base(factory) { }

        [HttpPost("GetIngresosInsumo")]
        public async Task<IActionResult> GetIngresosInsumo([FromBody] Paginacion paginacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            List<IngresoInsumo> lista = null;
            int totalRegistros = 0;

            using (var sn = factory.OpenSession())
            {
                IQueryable<IngresoInsumo> query = sn.Query<IngresoInsumo>();

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

                AsignarPaginacion(paginacion, ref query);

                query = query.OrderByDescending(x => x.FechaCreacion);

                lista = await query.ToListAsync();
            }

            return Ok(new
            {
                Lista = lista,
                TotalRegistros = totalRegistros
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetIngresoInsumo([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IngresoInsumo ingresoInsumo = null;

            using (var sn = factory.OpenSession())
            {
                ingresoInsumo = await sn.GetAsync<IngresoInsumo>(id);
                ingresoInsumo.Items = await sn.Query<ItemIngresoInsumo>().Where(x => x.IdIngresoInsumo == id).ToListAsync();
            }

            if (ingresoInsumo == null)
            {
                return NotFound();
            }

            return Ok(ingresoInsumo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutIngresoInsumo([FromRoute] int id, [FromBody] IngresoInsumo ingresoInsumo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ingresoInsumo.IdIngresoInsumo)
            {
                return BadRequest();
            }

            using (var sn = factory.OpenSession())
            {
                using (var tx = sn.BeginTransaction())
                {
                    try
                    {
                        decimal costo = ingresoInsumo.Items.Sum(x => x.Costo);

                        var ingresoInsumoBD = sn.Get<IngresoInsumo>(id);

                        var caja = await sn.GetAsync<Caja>(ingresoInsumo.Caja.IdCaja);

                        if ((ingresoInsumoBD.Costo + caja.Saldo) >= costo)
                        {
                            caja.Saldo += ingresoInsumoBD.Costo;
                            caja.Saldo -= costo;
                        }
                        else
                        {
                            throw new Exception("La caja no tiene saldo suficiente.");
                        }

                        var items = sn.Query<ItemIngresoInsumo>()
                            .Where(x => x.IdIngresoInsumo == id)
                            .ToList();

                        foreach (var item in items)
                        {
                            InventarioInsumo inventario = await sn.Query<InventarioInsumo>()
                                .Where(x => x.IdAlmacen == ingresoInsumo.Almacen.IdAlmacen
                                    && x.IdInsumo == item.Producto.IdProducto)
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

                        sn.Delete(string.Format("FROM ItemIngresoInsumo WHERE IdIngresoInsumo = {0}", id));

                        foreach (var item in ingresoInsumo.Items)
                        {
                            InventarioInsumo inventario = await sn.Query<InventarioInsumo>()
                                .Where(x => x.IdAlmacen == ingresoInsumo.Almacen.IdAlmacen
                                    && x.IdInsumo == item.Producto.IdProducto)
                                .FirstOrDefaultAsync();

                            if (inventario == null)
                            {
                                sn.Save(new InventarioInsumo
                                {
                                    IdAlmacen = ingresoInsumo.Almacen.IdAlmacen,
                                    IdInsumo = item.Producto.IdProducto,
                                    Stock = (item.Cantidad * item.Factor)
                                });
                            }
                            else
                            {
                                inventario.Stock = inventario.Stock + (item.Cantidad * item.Factor);
                            }

                            item.IdIngresoInsumo = id;

                            sn.Save(item);
                        }

                        MovimientoCaja movimiento = sn.Query<MovimientoCaja>()
                            .Where(x => x.IdIngresoInsumo == id).FirstOrDefault();

                        movimiento.Importe = costo * -1;

                        ingresoInsumoBD.Comentario = ingresoInsumo.Comentario;
                        ingresoInsumoBD.Costo = costo;

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
        public async Task<IActionResult> PostIngresoInsumo([FromBody] IngresoInsumo ingresoInsumo)
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
                        decimal costo = ingresoInsumo.Items.Sum(x => x.Costo);

                        var caja = await sn.GetAsync<Caja>(ingresoInsumo.Caja.IdCaja);

                        if (caja.Saldo >= costo)
                        {
                            caja.Saldo -= costo;
                        }
                        else
                        {
                            throw new Exception("La caja no tiene saldo suficiente.");
                        }

                        ingresoInsumo.FechaCreacion = DateTime.Now;
                        ingresoInsumo.Costo = costo;

                        sn.Save(ingresoInsumo);

                        foreach (var item in ingresoInsumo.Items)
                        {
                            InventarioInsumo inventario = await sn.Query<InventarioInsumo>()
                                .Where(x => x.IdAlmacen == ingresoInsumo.Almacen.IdAlmacen 
                                    && x.IdInsumo == item.Producto.IdProducto)
                                .FirstOrDefaultAsync();

                            if (inventario == null)
                            {
                                sn.Save(new InventarioInsumo
                                {
                                    IdAlmacen = ingresoInsumo.Almacen.IdAlmacen,
                                    IdInsumo = item.Producto.IdProducto,
                                    Stock = (item.Cantidad * item.Factor)
                                });
                            }
                            else
                            {
                                inventario.Stock = inventario.Stock + (item.Cantidad * item.Factor);
                            }

                            item.IdIngresoInsumo = ingresoInsumo.IdIngresoInsumo;

                            sn.Save(item);
                        }

                        sn.Save(new MovimientoCaja
                        {
                            IdCaja = caja.IdCaja,
                            Fecha = DateTime.Now,
                            IdIngresoInsumo = ingresoInsumo.IdIngresoInsumo,
                            TipoMovimiento = Constantes.Salida,
                            Importe = costo * -1,
                            Glosa = "COMPRA DE INSUMOS"
                        });

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
        public async Task<IActionResult> DeleteIngresoInsumo([FromRoute] int id)
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
                        IngresoInsumo ingresoInsumo = sn.Get<IngresoInsumo>(id);

                        List<ItemIngresoInsumo> items = await sn.Query<ItemIngresoInsumo>()
                            .Where(x => x.IdIngresoInsumo == id)
                            .ToListAsync();

                        foreach (var item in items)
                        {
                            InventarioInsumo inventario = await sn.Query<InventarioInsumo>()
                                .Where(x => x.IdAlmacen == ingresoInsumo.Almacen.IdAlmacen
                                    && x.IdInsumo == item.Producto.IdProducto)
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

                        var caja = await sn.GetAsync<Caja>(ingresoInsumo.Caja.IdCaja);

                        caja.Saldo += ingresoInsumo.Costo;

                        sn.Delete(string.Format("FROM MovimientoCaja WHERE IdIngresoInsumo = {0}", id));

                        sn.Delete(string.Format("FROM ItemIngresoInsumo WHERE IdIngresoInsumo = {0}", id));

                        sn.Delete(ingresoInsumo);

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