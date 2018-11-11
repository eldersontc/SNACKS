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
    public class MovimientosCajaController : UtilController
    {
        public MovimientosCajaController(ISessionFactory factory) : base(factory) { }

        [HttpPost("GetMovimientosCaja")]
        public async Task<IActionResult> GetMovimientosCaja([FromBody] Paginacion paginacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            List<MovimientoCaja> lista = null;
            int totalRegistros = 0;

            using (var sn = factory.OpenSession())
            {
                IQueryable<MovimientoCaja> query = sn.Query<MovimientoCaja>();

                foreach (Filtro filtro in paginacion.Filtros)
                {
                    switch (filtro.K)
                    {
                        case Constantes.Uno:
                            query = query.Where(x => x.IdCaja == filtro.N);
                            break;
                        case Constantes.Dos:
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

        [HttpPost]
        public async Task<IActionResult> PostMovimientoCaja([FromBody] MovimientoCaja movimientoCaja)
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
                        movimientoCaja.Fecha = DateTime.Now;

                        if (movimientoCaja.Importe > 0)
                        {
                            movimientoCaja.TipoMovimiento = Constantes.Ingreso;
                        }
                        else
                        {
                            movimientoCaja.TipoMovimiento = Constantes.Salida;
                        }

                        sn.Save(movimientoCaja);

                        Caja caja = sn.Get<Caja>(movimientoCaja.IdCaja);

                        caja.Saldo += movimientoCaja.Importe;

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