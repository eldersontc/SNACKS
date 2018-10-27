using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SNACKS.Data;
using SNACKS.Models;

namespace SNACKS.Controllers
{
    [Produces("application/json")]
    [Route("api/Pedidos")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        public IRepositorioBase<Pedido> Repositorio { get; }
        public IRepositorioBase<ItemPedido> RepositorioItem { get; }

        public PedidosController(IRepositorioBase<Pedido> repositorio,
            IRepositorioBase<ItemPedido> repositorioItem)
        {
            Repositorio = repositorio;
            RepositorioItem = repositorioItem;
        }

        [HttpPost("GetPedidos")]
        public async Task<IActionResult> GetPedidos([FromBody] Paginacion paginacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var filtros = new List<Expression<Func<Pedido, bool>>>();

            foreach (Filtro filtro in paginacion.Filtros)
            {
                switch (filtro.K)
                {
                    case Constantes.Uno:
                        filtros.Add(x => x.Cliente.IdPersona == filtro.N);
                        break;
                    case Constantes.Dos:
                        filtros.Add(x => x.FechaCreacion.Date == filtro.D.Date);
                        break;
                }
            }

            var result = await Repositorio.ObtenerTodosAsync(paginacion, filtros, new string[] { Constantes.Usuario, Constantes.Cliente });

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPedido([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pedido = await Repositorio.ObtenerAsync(id, new string[] {
                Constantes.Usuario,
                Constantes.Cliente,
                Constantes.Items + '.' + Constantes.Producto,
                Constantes.Items + '.' + Constantes.Unidad
            });

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

            try
            {
                List<ItemPedido> items = await RepositorioItem.ObtenerTodosAsync(
                    new List<Expression<Func<ItemPedido, bool>>>() {
                    (x => x.Pedido.IdPedido == id)
                });

                await RepositorioItem.EliminarAsync(items.ToArray(), false);

                List<object> referencias = new List<object>();
                decimal total = 0;

                foreach (var item in pedido.Items)
                {
                    item.Pedido = new Pedido { IdPedido = id };
                    referencias.Add(item.Producto);
                    referencias.Add(item.Unidad);
                    total += item.Total;
                }

                RepositorioItem.AgregarReferencias(referencias.ToArray());
                await RepositorioItem.RegistrarAsync(pedido.Items.ToArray(), false);

                pedido.Total = total;

                Repositorio.AgregarReferencias(new object[] { pedido, pedido.Usuario, pedido.Cliente });
                await Repositorio.ActualizarAsync(pedido);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
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

            try
            {
                List<object> referencias = new List<object>() { pedido.Usuario, pedido.Cliente };
                decimal total = 0;

                foreach (var item in pedido.Items)
                {
                    referencias.Add(item.Producto);
                    referencias.Add(item.Unidad);
                    total += item.Total;
                }

                pedido.FechaCreacion = DateTime.Now;
                pedido.Estado = Constantes.Pendiente;
                pedido.Total = total;

                Repositorio.AgregarReferencias(referencias.ToArray());
                await Repositorio.RegistrarAsync(pedido);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
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

            var pedido = await Repositorio.ObtenerAsync(id, new string[] { Constantes.Items });
            if (pedido == null)
            {
                return NotFound();
            }

            try
            {
                await RepositorioItem.EliminarAsync(pedido.Items.ToArray(), false);
                await Repositorio.EliminarAsync(new Pedido[] { pedido });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(true);
        }

        [HttpPost("Entregar/{id}")]
        public async Task<IActionResult> Entregar([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var pedido = await Repositorio.ObtenerAsync(id);
                pedido.FechaEntrega = DateTime.Now;
                pedido.Estado = Constantes.Entregado;
                await Repositorio.ActualizarAsync(pedido);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(true);
        }

        [HttpPost("Pagar/{id}")]
        public async Task<IActionResult> Pagar([FromRoute] int id, [FromBody] decimal pago)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var pedido = await Repositorio.ObtenerAsync(id);
                pedido.FechaPago = DateTime.Now;
                pedido.Pago += pago;
                if (pedido.Pago == pedido.Total)
                {
                    pedido.Estado = Constantes.Pagado;
                }
                else
                {
                    pedido.Estado = Constantes.PagoParcial;
                }
                await Repositorio.ActualizarAsync(pedido);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(true);
        }

    }
}