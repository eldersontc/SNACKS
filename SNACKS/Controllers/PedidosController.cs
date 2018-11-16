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
    [Route("api/Pedidos")]
    [ApiController]
    public class PedidosController : UtilController
    {
        public PedidosController(ISessionFactory factory) : base(factory) { }

        [HttpPost("GetPedidos")]
        public async Task<IActionResult> GetPedidos([FromBody] Paginacion paginacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            List<Pedido> lista = null;
            int totalRegistros = 0;

            using (var sn = factory.OpenSession())
            {
                IQueryable<Pedido> query = sn.Query<Pedido>();

                foreach (var filtro in paginacion.Filtros)
                {
                    switch (filtro.K)
                    {
                        case Constantes.Uno:
                            query = query.Where(x => x.Cliente.IdPersona == filtro.N);
                            break;
                        case Constantes.Dos:
                            query = query.Where(x => x.FechaCreacion.Date == filtro.D.Date);
                            break;
                        case Constantes.Tres:
                            query = query.Where(x => x.Cliente.IdPersona == filtro.N);
                            break;
                        case Constantes.Cuatro:
                            query = query.Where(x => x.Cliente.Vendedor.IdPersona == filtro.N);
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
        public async Task<IActionResult> GetPedido([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Pedido pedido = null;

            using (var sn = factory.OpenSession())
            {
                pedido = await sn.GetAsync<Pedido>(id);
                pedido.Items = await sn.Query<ItemPedido>().Where(x => x.IdPedido == id).ToListAsync();
            }

            if (pedido == null)
            {
                return NotFound();
            }

            return Ok(pedido);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPedido([FromRoute] int id, [FromBody] Pedido pedido)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pedido.IdPedido)
            {
                return BadRequest();
            }

            using (var sn = factory.OpenSession())
            {
                using (var tx = sn.BeginTransaction())
                {
                    try
                    {
                        sn.Delete(string.Format("FROM ItemPedido WHERE IdPedido = {0}", id));

                        Pedido pedidoBD = sn.Get<Pedido>(id);

                        pedidoBD.Comentario = pedido.Comentario;
                        pedidoBD.Total = pedido.Items.Sum(x => x.Total);

                        foreach (var item in pedido.Items)
                        {
                            item.IdPedido = pedido.IdPedido;

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

        [HttpPost]
        public async Task<IActionResult> PostPedido([FromBody] Pedido pedido)
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
                        pedido.FechaCreacion = DateTime.Now;
                        pedido.Estado = Constantes.Pendiente;
                        pedido.Total = pedido.Items.Sum(x => x.Total);

                        sn.Save(pedido);

                        foreach (var item in pedido.Items)
                        {
                            item.IdPedido = pedido.IdPedido;

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
        public async Task<IActionResult> DeletePedido([FromRoute] int id)
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
                        sn.Delete(string.Format("FROM ItemPedido WHERE IdPedido = {0}", id));

                        sn.Delete(new Pedido { IdPedido = id });

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

        [HttpPost("Entregar")]
        public async Task<IActionResult> Entregar([FromBody] SalidaProducto salidaProducto)
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
                        Pedido pedido = sn.Get<Pedido>(salidaProducto.IdPedido);

                        pedido.FechaEntrega = DateTime.Now;
                        pedido.Estado = Constantes.Entregado;

                        SalidaProducto salidaProductoBD = new SalidaProducto();

                        salidaProductoBD.FechaCreacion = DateTime.Now;
                        salidaProductoBD.Usuario = salidaProducto.Usuario;
                        salidaProductoBD.Almacen = salidaProducto.Almacen;
                        salidaProductoBD.Comentario = "GENERADO DESDE PEDIDO";
                        salidaProductoBD.IdPedido = salidaProducto.IdPedido;

                        sn.Save(salidaProductoBD);

                        List<ItemPedido> items = sn.Query<ItemPedido>()
                            .Where(x => x.IdPedido == pedido.IdPedido)
                            .ToList();

                        foreach (var item in items)
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
                                throw new Exception("No hay stock disponible.");
                            }

                            sn.Save(new ItemSalidaProducto {
                                IdSalidaProducto = salidaProductoBD.IdSalidaProducto,
                                Producto = item.Producto,
                                Unidad = item.Unidad,
                                Factor = item.Factor,
                                Cantidad = item.Cantidad
                            });
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

        [HttpPost("Pagar")]
        public async Task<IActionResult> Pagar([FromBody] MovimientoCaja movimientoCaja)
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
                        Pedido pedido = sn.Get<Pedido>(movimientoCaja.IdPedido);

                        pedido.FechaPago = DateTime.Now;
                        pedido.Pago += movimientoCaja.Importe;

                        if (pedido.Pago == pedido.Total)
                        {
                            pedido.Estado = Constantes.Pagado;
                        }
                        else
                        {
                            pedido.Estado = Constantes.PagoParcial;
                        }

                        Caja caja = sn.Get<Caja>(movimientoCaja.IdCaja);

                        caja.Saldo += movimientoCaja.Importe;

                        movimientoCaja.Fecha = DateTime.Now;
                        movimientoCaja.Glosa = "PAGO DE PEDIDO N° " + movimientoCaja.IdPedido;

                        sn.Save(movimientoCaja);

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