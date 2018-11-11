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
    [Route("api/SalidasInsumo")]
    [ApiController]
    public class SalidasInsumoController : UtilController
    {
        public SalidasInsumoController(ISessionFactory factory) : base(factory) { }

        [HttpPost("GetSalidasInsumo")]
        public async Task<IActionResult> GetSalidasInsumo([FromBody] Paginacion paginacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            List<SalidaInsumo> lista = null;
            int totalRegistros = 0;

            using (var sn = factory.OpenSession())
            {
                IQueryable<SalidaInsumo> query = sn.Query<SalidaInsumo>();

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
        public async Task<IActionResult> GetSalidaInsumo([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            SalidaInsumo salidaInsumo = null;

            using (var sn = factory.OpenSession())
            {
                salidaInsumo = await sn.GetAsync<SalidaInsumo>(id);
                salidaInsumo.Items = await sn.Query<ItemSalidaInsumo>().Where(x => x.IdSalidaInsumo == id).ToListAsync();
            }

            if (salidaInsumo == null)
            {
                return NotFound();
            }

            return Ok(salidaInsumo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutSalidaInsumo([FromRoute] int id, [FromBody] SalidaInsumo salidaInsumo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != salidaInsumo.IdSalidaInsumo)
            {
                return BadRequest();
            }

            using (var sn = factory.OpenSession())
            {
                using (var tx = sn.BeginTransaction())
                {
                    try
                    {
                        List<ItemSalidaInsumo> items = await sn.Query<ItemSalidaInsumo>()
                            .Where(x => x.IdSalidaInsumo == id)
                            .ToListAsync();

                        foreach (var item in items)
                        {
                            InventarioInsumo inventario = await sn.Query<InventarioInsumo>()
                                .Where(x => x.IdAlmacen == salidaInsumo.Almacen.IdAlmacen
                                    && x.IdInsumo == item.Insumo.IdProducto)
                                .FirstOrDefaultAsync();

                            inventario.Stock = inventario.Stock + (item.Cantidad * item.Factor);
                        }

                        sn.Delete(string.Format("FROM ItemSalidaInsumo WHERE IdSalidaInsumo = {0}", id));

                        foreach (var item in salidaInsumo.Items)
                        {
                            InventarioInsumo inventario = await sn.Query<InventarioInsumo>()
                                .Where(x => x.IdAlmacen == salidaInsumo.Almacen.IdAlmacen
                                    && x.IdInsumo == item.Insumo.IdProducto)
                                .FirstOrDefaultAsync();

                            if (inventario != null && inventario.Stock >= (item.Cantidad * item.Factor))
                            {
                                inventario.Stock = inventario.Stock - (item.Cantidad * item.Factor);
                            }
                            else
                            {
                                throw new Exception("No hay stock disponible.");
                            }

                            item.IdSalidaInsumo = id;

                            sn.Save(item);
                        }

                        sn.SaveOrUpdate(salidaInsumo);

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
        public async Task<IActionResult> PostSalidaInsumo([FromBody] SalidaInsumo salidaInsumo)
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
                        salidaInsumo.FechaCreacion = DateTime.Now;

                        sn.Save(salidaInsumo);

                        foreach (var item in salidaInsumo.Items)
                        {
                            InventarioInsumo inventario = await sn.Query<InventarioInsumo>()
                                .Where(x => x.IdAlmacen == salidaInsumo.Almacen.IdAlmacen
                                    && x.IdInsumo == item.Insumo.IdProducto)
                                .FirstOrDefaultAsync();

                            if (inventario != null && inventario.Stock >= (item.Cantidad * item.Factor))
                            {
                                inventario.Stock = inventario.Stock - (item.Cantidad * item.Factor);
                            }
                            else
                            {
                                throw new Exception("No hay stock disponible.");
                            }

                            item.IdSalidaInsumo = salidaInsumo.IdSalidaInsumo;

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
        public async Task<IActionResult> DeleteSalidaInsumo([FromRoute] int id)
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
                        SalidaInsumo salidaInsumo = sn.Get<SalidaInsumo>(id);

                        List<ItemSalidaInsumo> items = await sn.Query<ItemSalidaInsumo>()
                            .Where(x => x.IdSalidaInsumo == id)
                            .ToListAsync();

                        foreach (var item in items)
                        {
                            InventarioInsumo inventario = await sn.Query<InventarioInsumo>()
                                .Where(x => x.IdAlmacen == salidaInsumo.Almacen.IdAlmacen
                                    && x.IdInsumo == item.Insumo.IdProducto)
                                .FirstOrDefaultAsync();

                            inventario.Stock = inventario.Stock + (item.Cantidad * item.Factor);
                        }

                        sn.Delete(string.Format("FROM ItemSalidaInsumo WHERE IdSalidaInsumo = {0}", id));

                        sn.Delete(salidaInsumo);

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