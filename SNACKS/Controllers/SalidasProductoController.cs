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
    [Route("api/SalidasProducto")]
    [ApiController]
    public class SalidasProductoController : ControllerBase
    {
        public IRepositorioBase<SalidaProducto> Repositorio { get; }
        public IRepositorioBase<ItemSalidaProducto> RepositorioItem { get; }
        public IRepositorioBase<InventarioProducto> RepositorioInventario { get; }

        public SalidasProductoController(IRepositorioBase<SalidaProducto> repositorio,
            IRepositorioBase<ItemSalidaProducto> repositorioItem,
            IRepositorioBase<InventarioProducto> repositorioInventario)
        {
            Repositorio = repositorio;
            RepositorioItem = repositorioItem;
            RepositorioInventario = repositorioInventario;
        }

        [HttpPost("GetSalidasProducto")]
        public async Task<IActionResult> GetSalidasProducto([FromBody] Paginacion paginacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var filtros = new List<Expression<Func<SalidaProducto, bool>>>();

            foreach (Filtro filtro in paginacion.Filtros)
            {
                switch (filtro.K)
                {
                    case Constantes.Uno:
                        filtros.Add(x => x.FechaCreacion.Date == filtro.D.Date);
                        break;
                }
            }

            var result = await Repositorio.ObtenerTodosAsync(paginacion, filtros, new string[] { Constantes.Usuario }, (x => x.FechaCreacion));

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSalidaProducto([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ingresoProducto = await Repositorio.ObtenerAsync(id, new string[] {
                Constantes.Usuario,
                Constantes.Items + '.' + Constantes.Producto,
                Constantes.Items + '.' + Constantes.Unidad
            });

            if (ingresoProducto == null)
            {
                return NotFound();
            }

            return Ok(ingresoProducto);
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

            try
            {
                /* Eliminación de los anteriores items */
                List<ItemSalidaProducto> items = await RepositorioItem.ObtenerTodosAsync(
                    new List<Expression<Func<ItemSalidaProducto, bool>>>() {
                    (x => x.SalidaProducto.IdSalidaProducto == id)
                },
                    new string[] {
                    Constantes.Producto,
                    Constantes.Unidad
                });

                foreach (var item in items)
                {
                    var filtros = new List<Expression<Func<InventarioProducto, bool>>>() {
                        (x => x.IdProducto == item.Producto.IdProducto)
                    };
                    List<InventarioProducto> inventarios = await RepositorioInventario.ObtenerTodosAsync(filtros);
                    if (inventarios.Count > 0)
                    {
                        inventarios[0].Stock = inventarios[0].Stock + (item.Cantidad * item.Factor);
                        await RepositorioInventario.ActualizarAsync(inventarios[0], Confirmar: false);
                    }
                    else
                    {
                        return StatusCode(500, "No hay stock disponible.");
                    }
                }

                await RepositorioItem.EliminarAsync(items.ToArray(), false);

                /* Registro de los nuevos items */
                List<object> referencias = new List<object>();

                foreach (var item in salidaProducto.Items)
                {
                    item.SalidaProducto = new SalidaProducto { IdSalidaProducto = id };
                    referencias.Add(item.Producto);
                    referencias.Add(item.Unidad);
                }

                RepositorioItem.AgregarReferencias(referencias.ToArray());
                await RepositorioItem.RegistrarAsync(salidaProducto.Items.ToArray(), false);

                foreach (var item in salidaProducto.Items)
                {
                    var filtros = new List<Expression<Func<InventarioProducto, bool>>>() {
                        (x => x.IdProducto == item.Producto.IdProducto)
                    };
                    List<InventarioProducto> inventarios = await RepositorioInventario.ObtenerTodosAsync(filtros);
                    if (inventarios.Count > 0 && inventarios[0].Stock >= (item.Cantidad * item.Factor))
                    {
                        inventarios[0].Stock = inventarios[0].Stock - (item.Cantidad * item.Factor);
                        await RepositorioInventario.ActualizarAsync(inventarios[0], Confirmar: false);
                    }
                    else
                    {
                        return StatusCode(500, "No hay stock disponible.");
                    }
                }

                /* Actualización de la cabecera */
                Repositorio.AgregarReferencias(new object[] { salidaProducto, salidaProducto.Usuario });
                await Repositorio.ActualizarAsync(salidaProducto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
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

            try
            {
                foreach (var item in salidaProducto.Items)
                {
                    var filtros = new List<Expression<Func<InventarioProducto, bool>>>() {
                        (x => x.IdProducto == item.Producto.IdProducto)
                    };
                    List<InventarioProducto> inventarios = await RepositorioInventario.ObtenerTodosAsync(filtros);
                    if (inventarios.Count > 0 && inventarios[0].Stock >= (item.Cantidad * item.Factor))
                    {
                        inventarios[0].Stock = inventarios[0].Stock - (item.Cantidad * item.Factor);
                        await RepositorioInventario.ActualizarAsync(inventarios[0], Confirmar: false);
                    }
                    else
                    {
                        return StatusCode(500, "No hay stock disponible.");
                    }
                }

                List<object> referencias = new List<object>() { salidaProducto.Usuario };

                foreach (var item in salidaProducto.Items)
                {
                    referencias.Add(item.Producto);
                    referencias.Add(item.Unidad);
                }

                salidaProducto.FechaCreacion = DateTime.Now;

                Repositorio.AgregarReferencias(referencias.ToArray());
                await Repositorio.RegistrarAsync(salidaProducto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
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

            var salidaProducto = await Repositorio.ObtenerAsync(id, new string[] {
                Constantes.Items + '.' + Constantes.Producto
            });
            if (salidaProducto == null)
            {
                return NotFound();
            }

            try
            {
                foreach (var item in salidaProducto.Items)
                {
                    var filtros = new List<Expression<Func<InventarioProducto, bool>>>() {
                        (x => x.IdProducto == item.Producto.IdProducto)
                    };
                    List<InventarioProducto> inventarios = await RepositorioInventario.ObtenerTodosAsync(filtros);
                    if (inventarios.Count > 0)
                    {
                        inventarios[0].Stock = inventarios[0].Stock + (item.Cantidad * item.Factor);
                        await RepositorioInventario.ActualizarAsync(inventarios[0], Confirmar: false);
                    }
                    else
                    {
                        return StatusCode(500, "No hay stock disponible.");
                    }
                }
                await RepositorioItem.EliminarAsync(salidaProducto.Items.ToArray(), false);
                await Repositorio.EliminarAsync(salidaProducto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(true);
        }

    }
}