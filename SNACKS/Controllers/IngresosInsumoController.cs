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
    [Route("api/IngresosInsumo")]
    [ApiController]
    public class IngresosInsumoController : ControllerBase
    {
        public IRepositorioBase<IngresoInsumo> Repositorio { get; }
        public IRepositorioBase<ItemIngresoInsumo> RepositorioItem { get; }
        public IRepositorioBase<InventarioInsumo> RepositorioInventario { get; }

        public IngresosInsumoController(IRepositorioBase<IngresoInsumo> repositorio,
            IRepositorioBase<ItemIngresoInsumo> repositorioItem,
            IRepositorioBase<InventarioInsumo> repositorioInventario)
        {
            Repositorio = repositorio;
            RepositorioItem = repositorioItem;
            RepositorioInventario = repositorioInventario;
        }

        [HttpPost("GetIngresosInsumo")]
        public async Task<IActionResult> GetIngresosInsumo([FromBody] Paginacion paginacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var filtros = new List<Expression<Func<IngresoInsumo, bool>>>();

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
        public async Task<IActionResult> GetIngresoInsumo([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ingresoInsumo = await Repositorio.ObtenerAsync(id, new string[] {
                Constantes.Usuario,
                Constantes.Items + '.' + Constantes.Producto,
                Constantes.Items + '.' + Constantes.Unidad
            });

            if (ingresoInsumo == null)
            {
                return NotFound();
            }

            return Ok(ingresoInsumo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutIngresoInsumo([FromRoute] int id, [FromBody] IngresoInsumo ingresoInsumo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ingresoInsumo.IdIngresoInsumo)
            {
                return BadRequest();
            }

            try
            {
                /* Eliminación de los anteriores items */
                List<ItemIngresoInsumo> items = await RepositorioItem.ObtenerTodosAsync(
                    new List<Expression<Func<ItemIngresoInsumo, bool>>>() {
                    (x => x.IngresoInsumo.IdIngresoInsumo == id)
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

                await RepositorioItem.EliminarAsync(items.ToArray(), false);

                /* Registro de los nuevos items */
                List<object> referencias = new List<object>();
                decimal costo = 0;

                foreach (var item in ingresoInsumo.Items)
                {
                    item.IngresoInsumo = new IngresoInsumo { IdIngresoInsumo = id };
                    referencias.Add(item.Producto);
                    referencias.Add(item.Unidad);
                    costo += item.Costo;
                }

                RepositorioItem.AgregarReferencias(referencias.ToArray());
                await RepositorioItem.RegistrarAsync(ingresoInsumo.Items.ToArray(), false);

                foreach (var item in ingresoInsumo.Items)
                {
                    var filtros = new List<Expression<Func<InventarioInsumo, bool>>>() {
                        (x => x.IdInsumo == item.Producto.IdProducto)
                    };
                    List<InventarioInsumo> inventarios = await RepositorioInventario.ObtenerTodosAsync(filtros);
                    if (inventarios.Count == 0)
                    {
                        await RepositorioInventario.RegistrarAsync(new InventarioInsumo
                        {
                            IdInsumo = item.Producto.IdProducto,
                            Stock = (item.Cantidad * item.Factor)
                        }, Confirmar: false);
                    }
                    else
                    {
                        inventarios[0].Stock = inventarios[0].Stock + (item.Cantidad * item.Factor);
                        await RepositorioInventario.ActualizarAsync(inventarios[0], Confirmar: false);
                    }
                }

                /* Actualización de la cabecera */
                ingresoInsumo.Costo = costo;

                Repositorio.AgregarReferencias(new object[] { ingresoInsumo, ingresoInsumo.Usuario });
                await Repositorio.ActualizarAsync(ingresoInsumo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(true);
        }

        [HttpPost]
        public async Task<IActionResult> PostIngresoInsumo([FromBody] IngresoInsumo ingresoInsumo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                foreach (var item in ingresoInsumo.Items)
                {
                    var filtros = new List<Expression<Func<InventarioInsumo, bool>>>() {
                        (x => x.IdInsumo == item.Producto.IdProducto)
                    };
                    List<InventarioInsumo> inventarios = await RepositorioInventario.ObtenerTodosAsync(filtros);
                    if (inventarios.Count == 0)
                    {
                        await RepositorioInventario.RegistrarAsync(new InventarioInsumo
                        {
                            IdInsumo = item.Producto.IdProducto,
                            Stock = (item.Cantidad * item.Factor)
                        }, false);
                    }
                    else
                    {
                        inventarios[0].Stock = inventarios[0].Stock + (item.Cantidad * item.Factor);
                        await RepositorioInventario.ActualizarAsync(inventarios[0], Confirmar: false);
                    }
                }

                List<object> referencias = new List<object>() { ingresoInsumo.Usuario };
                decimal costo = 0;

                foreach (var item in ingresoInsumo.Items)
                {
                    referencias.Add(item.Producto);
                    referencias.Add(item.Unidad);
                    costo += item.Costo;
                }

                ingresoInsumo.FechaCreacion = DateTime.Now;
                ingresoInsumo.Costo = costo;

                Repositorio.AgregarReferencias(referencias.ToArray());
                await Repositorio.RegistrarAsync(ingresoInsumo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(true);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIngresoInsumo([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ingresoInsumo = await Repositorio.ObtenerAsync(id, new string[] {
                Constantes.Items + '.' + Constantes.Producto
            });
            if (ingresoInsumo == null)
            {
                return NotFound();
            }

            try
            {
                foreach (var item in ingresoInsumo.Items)
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
                await RepositorioItem.EliminarAsync(ingresoInsumo.Items.ToArray(), false);
                await Repositorio.EliminarAsync(ingresoInsumo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(true);
        }

        //[HttpPost("AddItem")]
        //public async Task<IActionResult> PostItem([FromBody] ItemIngresoInsumo item)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    try
        //    {
        //        await RepositorioItem.RegistrarAsync(item, new object[] { item.IngresoInsumo, item.Producto, item.Unidad });
        //        var filtros = new List<Expression<Func<InventarioInsumo, bool>>>() {
        //                (x => x.IdInsumo == item.Producto.IdProducto),
        //                (x => x.IdUnidad == item.Unidad.IdUnidad),
        //                (x => x.Factor == item.Factor)
        //            };
        //        List<InventarioInsumo> inventarios = await RepositorioInventario.ObtenerTodosAsync(filtros);
        //        if (inventarios.Count == 0)
        //        {
        //            await RepositorioInventario.RegistrarAsync(new InventarioInsumo
        //            {
        //                IdInsumo = item.Producto.IdProducto,
        //                IdUnidad = item.Unidad.IdUnidad,
        //                Factor = item.Factor,
        //                Stock = item.Cantidad
        //            });
        //        }
        //        else
        //        {
        //            inventarios[0].Stock = inventarios[0].Stock + item.Cantidad;
        //            await RepositorioInventario.ActualizarAsync(inventarios[0]);
        //        }
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

        //    var item = await RepositorioItem.ObtenerAsync(id, new string[] {
        //        Constantes.Producto,
        //        Constantes.Unidad
        //    });
        //    if (item == null)
        //    {
        //        return NotFound();
        //    }

        //    try
        //    {
        //        await RepositorioItem.EliminarAsync(new ItemIngresoInsumo[] { item });
        //        var filtros = new List<Expression<Func<InventarioInsumo, bool>>>() {
        //                (x => x.IdInsumo == item.Producto.IdProducto),
        //                (x => x.IdUnidad == item.Unidad.IdUnidad),
        //                (x => x.Factor == item.Factor)
        //            };
        //        List<InventarioInsumo> inventarios = await RepositorioInventario.ObtenerTodosAsync(filtros);
        //        if (inventarios.Count > 0)
        //        {
        //            inventarios[0].Stock = inventarios[0].Stock - item.Cantidad;
        //            await RepositorioInventario.ActualizarAsync(inventarios[0]);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }

        //    return Ok(true);
        //}
    }
}