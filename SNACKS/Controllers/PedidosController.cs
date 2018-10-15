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
    [Route("api/Pedidos")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        public IRepositorioBase<Pedido> Repositorio { get; }

        public PedidosController(IRepositorioBase<Pedido> repositorio)
        {
            Repositorio = repositorio;
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

            var result = await Repositorio.ObtenerTodosAsync(paginacion, filtros, new string[] { Constantes.Cliente });

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPedido([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pedido = await Repositorio.ObtenerAsync(id, new string[] { Constantes.Cliente });

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
                await Repositorio.ActualizarAsync(pedido, new object[] { pedido.Cliente });
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
                pedido.FechaCreacion = DateTime.Now;
                await Repositorio.RegistrarAsync(pedido, new object[] { pedido.Cliente });
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

            var pedido = await Repositorio.ObtenerAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }

            try
            {
                await Repositorio.EliminarAsync(new Pedido[] { pedido });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(true);
        }
    }
}