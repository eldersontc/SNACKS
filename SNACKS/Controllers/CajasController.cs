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
    [Route("api/[controller]")]
    [ApiController]
    public class CajasController : UtilController
    {
        public CajasController(ISessionFactory factory) : base(factory) { }

        [HttpPost("GetCajas")]
        public async Task<IActionResult> GetCajas([FromBody] Paginacion paginacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            List<Caja> lista = null;
            int totalRegistros = 0;

            using (var sn = factory.OpenSession())
            {
                IQueryable<Caja> query = sn.Query<Caja>();

                foreach (Filtro filtro in paginacion.Filtros)
                {
                    switch (filtro.K)
                    {
                        case Constantes.Uno:
                            query = query.Where(x => x.Nombre.Contains(filtro.V));
                            break;
                    }
                }

                totalRegistros = await query.CountAsync();

                AsignarPaginacion(paginacion, ref query);

                query = query.OrderBy(x => x.Nombre);

                lista = await query.ToListAsync();
            }

            return Ok(new
            {
                Lista = lista,
                TotalRegistros = totalRegistros
            });
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            List<Caja> lista = null;

            using (var sn = factory.OpenSession())
            {
                lista = await sn.Query<Caja>().ToListAsync();
            }

            return Ok(lista);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCaja([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Caja caja = null;

            using (var sn = factory.OpenSession())
            {
                caja = await sn.GetAsync<Caja>(id);
            }

            if (caja == null)
            {
                return NotFound();
            }

            return Ok(caja);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCaja([FromRoute] int id, [FromBody] Caja caja)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != caja.IdCaja)
            {
                return BadRequest();
            }

            using (var sn = factory.OpenSession())
            {
                using (var tx = sn.BeginTransaction())
                {
                    try
                    {
                        Caja cajaBD = sn.Get<Caja>(caja.IdCaja);

                        cajaBD.Nombre = caja.Nombre;
                        
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
        public async Task<IActionResult> PostCaja([FromBody] Caja caja)
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
                        sn.Save(caja);

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
        public async Task<IActionResult> DeleteCaja([FromRoute] int id)
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
                        sn.Delete(new Caja { IdCaja = id });

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