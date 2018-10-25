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
    [Route("api/SalidasInsumo")]
    [ApiController]
    public class SalidasInsumoController : ControllerBase
    {
        public IRepositorioBase<SalidaInsumo> Repositorio { get; }
        public IRepositorioBase<ItemSalidaInsumo> RepositorioItem { get; }
        public IRepositorioBase<InventarioInsumo> RepositorioInventario { get; }

        public SalidasInsumoController(IRepositorioBase<SalidaInsumo> repositorio,
            IRepositorioBase<ItemSalidaInsumo> repositorioItem,
            IRepositorioBase<InventarioInsumo> repositorioInventario)
        {
            Repositorio = repositorio;
            RepositorioItem = repositorioItem;
            RepositorioInventario = repositorioInventario;
        }

        [HttpPost("GetSalidasInsumo")]
        public async Task<IActionResult> GetSalidasInsumo([FromBody] Paginacion paginacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var filtros = new List<Expression<Func<SalidaInsumo, bool>>>();

            foreach (Filtro filtro in paginacion.Filtros)
            {
                switch (filtro.K)
                {
                    case Constantes.Uno:
                        filtros.Add(x => x.FechaCreacion.Date == filtro.D.Date);
                        break;
                }
            }

            var result = await Repositorio.ObtenerTodosAsync(paginacion, filtros, new string[] { Constantes.Usuario });

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSalidaInsumo([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var salidaInsumo = await Repositorio.ObtenerAsync(id, new string[] {
                Constantes.Usuario,
                Constantes.Items + '.' + Constantes.Producto,
                Constantes.Items + '.' + Constantes.Unidad
            });

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

            try
            {
                /* Eliminación de los anteriores items */
                List<ItemSalidaInsumo> items = await RepositorioItem.ObtenerTodosAsync(
                    new List<Expression<Func<ItemSalidaInsumo, bool>>>() {
                    (x => x.SalidaInsumo.IdSalidaInsumo == id)
                },
                    new string[] {
                    Constantes.Producto,
                    Constantes.Unidad
                });

                foreach (var item in items)
                {
                    var filtros = new List<Expression<Func<InventarioInsumo, bool>>>() {
                        (x => x.IdInsumo == item.Producto.IdProducto)
                    };
                    List<InventarioInsumo> inventarios = await RepositorioInventario.ObtenerTodosAsync(filtros);
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
                
                foreach (var item in salidaInsumo.Items)
                {
                    item.SalidaInsumo = new SalidaInsumo { IdSalidaInsumo = id };
                    referencias.Add(item.Producto);
                    referencias.Add(item.Unidad);
                }

                RepositorioItem.AgregarReferencias(referencias.ToArray());
                await RepositorioItem.RegistrarAsync(salidaInsumo.Items.ToArray(), false);

                foreach (var item in salidaInsumo.Items)
                {
                    var filtros = new List<Expression<Func<InventarioInsumo, bool>>>() {
                        (x => x.IdInsumo == item.Producto.IdProducto)
                    };
                    List<InventarioInsumo> inventarios = await RepositorioInventario.ObtenerTodosAsync(filtros);
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
                Repositorio.AgregarReferencias(new object[] { salidaInsumo, salidaInsumo.Usuario });
                await Repositorio.ActualizarAsync(salidaInsumo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
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

            try
            {
                foreach (var item in salidaInsumo.Items)
                {
                    var filtros = new List<Expression<Func<InventarioInsumo, bool>>>() {
                        (x => x.IdInsumo == item.Producto.IdProducto)
                    };
                    List<InventarioInsumo> inventarios = await RepositorioInventario.ObtenerTodosAsync(filtros);
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

                List<object> referencias = new List<object>() { salidaInsumo.Usuario };
                
                foreach (var item in salidaInsumo.Items)
                {
                    referencias.Add(item.Producto);
                    referencias.Add(item.Unidad);
                }

                salidaInsumo.FechaCreacion = DateTime.Now;
                
                Repositorio.AgregarReferencias(referencias.ToArray());
                await Repositorio.RegistrarAsync(salidaInsumo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
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

            var salidaInsumo = await Repositorio.ObtenerAsync(id, new string[] {
                Constantes.Items + '.' + Constantes.Producto
            });
            if (salidaInsumo == null)
            {
                return NotFound();
            }

            try
            {
                foreach (var item in salidaInsumo.Items)
                {
                    var filtros = new List<Expression<Func<InventarioInsumo, bool>>>() {
                        (x => x.IdInsumo == item.Producto.IdProducto)
                    };
                    List<InventarioInsumo> inventarios = await RepositorioInventario.ObtenerTodosAsync(filtros);
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
                await RepositorioItem.EliminarAsync(salidaInsumo.Items.ToArray(), false);
                await Repositorio.EliminarAsync(salidaInsumo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(true);
        }

        //[HttpPost("AddItem")]
        //public async Task<IActionResult> PostItem([FromBody] ItemSalidaInsumo item)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    try
        //    {
        //        await RepositorioItem.RegistrarAsync(item, new object[] { item.SalidaInsumo, item.Producto, item.Unidad });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }

        //    return Ok(true);
        //}

        //[HttpDelete("DeleteItem/{id}")]
        //public async Task<IActionResult> DeleteItem([FromRoute] int id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var item = await RepositorioItem.ObtenerAsync(id);
        //    if (item == null)
        //    {
        //        return NotFound();
        //    }

        //    try
        //    {
        //        await RepositorioItem.EliminarAsync(new ItemSalidaInsumo[] { item });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }

        //    return Ok(true);
        //}
    }
}