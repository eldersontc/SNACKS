using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Transform;
using SNACKS.Models;

namespace SNACKS.Controllers
{
    [Produces("application/json")]
    [Route("api/Reportes")]
    [ApiController]
    public class ReportesController : UtilController
    {
        public ReportesController(ISessionFactory factory) : base(factory) { }

        [HttpPost("GetReportes")]
        public async Task<IActionResult> GetReportes([FromBody] Paginacion paginacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            List<Reporte> lista = null;
            int totalRegistros = 0;

            using (var sn = factory.OpenSession())
            {
                IQueryable<Reporte> query = sn.Query<Reporte>();

                foreach (Filtro filtro in paginacion.Filtros)
                {
                    switch (filtro.K)
                    {
                        case Constantes.Uno:
                            query = query.Where(x => x.Titulo.Contains(filtro.V));
                            break;
                    }
                }

                totalRegistros = await query.CountAsync();

                query = query.OrderBy(x => x.Titulo);

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
        public async Task<IActionResult> GetReporte([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Reporte reporte = null;

            using (var sn = factory.OpenSession())
            {
                reporte = await sn.GetAsync<Reporte>(id);
                reporte.Items = await sn.Query<ItemReporte>().Where(x => x.IdReporte == id).ToListAsync();
            }

            if (reporte == null)
            {
                return NotFound();
            }

            return Ok(reporte);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutReporte([FromRoute] int id, [FromBody] Reporte reporte)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != reporte.IdReporte)
            {
                return BadRequest();
            }

            using (var sn = factory.OpenSession())
            {
                using (var tx = sn.BeginTransaction())
                {
                    try
                    {
                        sn.Delete(string.Format("FROM ItemReporte WHERE IdReporte = {0}", id));

                        foreach (var item in reporte.Items)
                        {
                            item.IdReporte = id;
                            sn.Save(item);
                        }

                        sn.SaveOrUpdate(reporte);

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
        public async Task<IActionResult> Post([FromBody] Reporte reporte)
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
                        sn.Save(reporte);

                        foreach (var item in reporte.Items)
                        {
                            item.IdReporte = reporte.IdReporte;
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
        public async Task<IActionResult> DeleteReporte([FromRoute] int id)
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
                        sn.Delete(string.Format("FROM ItemReporte WHERE IdReporte = {0}", id));

                        sn.Delete(new Reporte { IdReporte = id });

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

        [HttpPost("RunReporte")]
        public async Task<IActionResult> RunReporte([FromBody] Reporte reporte)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string sql = string.Format("Usp_Estadisticas {0}", reporte.Flag);
            
            foreach (var item in reporte.Items)
            {
                sql += string.Format(", '{0}'", item.Valor);
            }

            IList<Estadistica> lista;

            try
            {
                using (var sn = factory.OpenSession())
                {
                    lista = await sn.CreateSQLQuery(sql)
                        .SetResultTransformer(Transformers.AliasToBean<Estadistica>())
                        .ListAsync<Estadistica>();
                    return Ok(lista);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}