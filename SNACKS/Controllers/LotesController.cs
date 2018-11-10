using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NHibernate;
using NHibernate.Linq;
using SNACKS.Models;

namespace SNACKS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LotesController : UtilController
    {
        public LotesController(ISessionFactory factory) : base(factory) { }

        [HttpPost("GetLotes")]
        public async Task<IActionResult> GetLotes([FromBody] Paginacion paginacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            List<Lote> lista = null;
            int totalRegistros = 0;

            using (var sn = factory.OpenSession())
            {
                IQueryable<Lote> query = sn.Query<Lote>();

                foreach (Filtro filtro in paginacion.Filtros)
                {
                    switch (filtro.K)
                    {
                        case Constantes.Uno:
                            query = query.Where(x => x.Fecha.Date == filtro.D.Date);
                            break;
                    }
                }

                totalRegistros = await query.CountAsync();

                AsignarPaginacion(paginacion, ref query);

                query = query.OrderByDescending(x => x.Fecha);

                lista = await query.ToListAsync();
            }

            return Ok(new
            {
                Lista = lista,
                TotalRegistros = totalRegistros
            });
        }

        [HttpGet("GetItemsOnly/{id}")]
        public async Task<IActionResult> GetItemsOnly([FromRoute] int id)
        {
            List<ItemLote> lista = null;

            using (var sn = factory.OpenSession())
            {
                lista = await sn.Query<ItemLote>().Where(x => x.IdLote == id).ToListAsync();
            }

            return Ok(lista);
        }

        [HttpGet("GetItems/{id}")]
        public async Task<IActionResult> GetItems([FromRoute] int id)
        {
            List<ItemLote> lista = null;

            using (var sn = factory.OpenSession())
            {
                lista = await sn.Query<ItemLote>()
                    .Where(x => x.IdLote == id).ToListAsync();

                foreach (var item in lista)
                {
                    item.Producto.Items = sn.Query<ItemProducto>()
                        .Where(x => x.IdProducto == item.Producto.IdProducto)
                        .ToList();
                }
            }

            return Ok(lista);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLote([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Lote lote = null;

            using (var sn = factory.OpenSession())
            {
                lote = await sn.GetAsync<Lote>(id);
                lote.Items = await sn.Query<ItemLote>().Where(x => x.IdLote == id).ToListAsync();
            }

            if (lote == null)
            {
                return NotFound();
            }

            return Ok(lote);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutLote([FromRoute] int id, [FromBody] Lote lote)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != lote.IdLote)
            {
                return BadRequest();
            }

            using (var sn = factory.OpenSession())
            {
                using (var tx = sn.BeginTransaction())
                {
                    try
                    {
                        sn.Delete(string.Format("FROM ItemLote WHERE IdLote = {0}", id));

                        foreach (var item in lote.Items)
                        {
                            item.IdLote = id;
                            sn.Save(item);
                        }

                        sn.SaveOrUpdate(lote);

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
        public async Task<IActionResult> Post([FromBody] Lote lote)
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
                        lote.Fecha = DateTime.Now;

                        sn.Save(lote);

                        foreach (var item in lote.Items)
                        {
                            item.IdLote = lote.IdLote;
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
        public async Task<IActionResult> DeleteLote([FromRoute] int id)
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
                        sn.Delete(string.Format("FROM ItemLote WHERE IdLote = {0}", id));

                        sn.Delete(new Lote { IdLote = id });

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