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
    [Route("api/IngresosProducto")]
    [ApiController]
    public class IngresosProductoController : ControllerBase
    {
        public IRepositorioBase<IngresoProducto> Repositorio { get; }
        public IRepositorioBase<ItemIngresoProducto> RepositorioItem { get; }
        public IRepositorioBase<InventarioProducto> RepositorioInventario { get; }

        public IngresosProductoController(IRepositorioBase<IngresoProducto> repositorio,
            IRepositorioBase<ItemIngresoProducto> repositorioItem,
            IRepositorioBase<InventarioProducto> repositorioInventario)
        {
            Repositorio = repositorio;
            RepositorioItem = repositorioItem;
            RepositorioInventario = repositorioInventario;
        }

        [HttpPost("GetIngresosProducto")]
        public async Task<IActionResult> GetIngresosProducto([FromBody] Paginacion paginacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var filtros = new List<Expression<Func<IngresoProducto, bool>>>();

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
        public async Task<IActionResult> GetIngresoProducto([FromRoute] int id)
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
        public async Task<IActionResult> PutIngresoProducto([FromRoute] int id, [FromBody] IngresoProducto ingresoProducto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ingresoProducto.IdIngresoProducto)
            {
                return BadRequest();
            }

            try
            {
                /* Eliminación de los anteriores items */
                List<ItemIngresoProducto> items = await RepositorioItem.ObtenerTodosAsync(
                    new List<Expression<Func<ItemIngresoProducto, bool>>>() {
                    (x => x.IngresoProducto.IdIngresoProducto == id)
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
                    if (inventarios.Count > 0 && inventarios[0].Stock >= (item.Cantidad * item.Factor))
                    {
                        inventarios[0].Stock = inventarios[0].Stock - (item.Cantidad * item.Cantidad);
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
                
                foreach (var item in ingresoProducto.Items)
                {
                    item.IngresoProducto = new IngresoProducto { IdIngresoProducto = id };
                    referencias.Add(item.Producto);
                    referencias.Add(item.Unidad);
                }

                RepositorioItem.AgregarReferencias(referencias.ToArray());
                await RepositorioItem.RegistrarAsync(ingresoProducto.Items.ToArray(), false);

                foreach (var item in ingresoProducto.Items)
                {
                    var filtros = new List<Expression<Func<InventarioProducto, bool>>>() {
                        (x => x.IdProducto == item.Producto.IdProducto)
                    };
                    List<InventarioProducto> inventarios = await RepositorioInventario.ObtenerTodosAsync(filtros);
                    if (inventarios.Count == 0)
                    {
                        await RepositorioInventario.RegistrarAsync(new InventarioProducto
                        {
                            IdProducto = item.Producto.IdProducto,
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
                Repositorio.AgregarReferencias(new object[] { ingresoProducto, ingresoProducto.Usuario });
                await Repositorio.ActualizarAsync(ingresoProducto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(true);
        }

        [HttpPost]
        public async Task<IActionResult> PostIngresoProducto([FromBody] IngresoProducto ingresoProducto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                foreach (var item in ingresoProducto.Items)
                {
                    var filtros = new List<Expression<Func<InventarioProducto, bool>>>() {
                        (x => x.IdProducto == item.Producto.IdProducto)
                    };
                    List<InventarioProducto> inventarios = await RepositorioInventario.ObtenerTodosAsync(filtros);
                    if (inventarios.Count == 0)
                    {
                        await RepositorioInventario.RegistrarAsync(new InventarioProducto
                        {
                            IdProducto = item.Producto.IdProducto,
                            Stock = (item.Cantidad * item.Factor)
                        }, false);
                    }
                    else
                    {
                        inventarios[0].Stock = inventarios[0].Stock + (item.Cantidad * item.Factor);
                        await RepositorioInventario.ActualizarAsync(inventarios[0], Confirmar: false);
                    }
                }

                List<object> referencias = new List<object>() { ingresoProducto.Usuario };
                
                foreach (var item in ingresoProducto.Items)
                {
                    referencias.Add(item.Producto);
                    referencias.Add(item.Unidad);
                }

                ingresoProducto.FechaCreacion = DateTime.Now;

                Repositorio.AgregarReferencias(referencias.ToArray());
                await Repositorio.RegistrarAsync(ingresoProducto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(true);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIngresoProducto([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ingresoProducto = await Repositorio.ObtenerAsync(id, new string[] {
                Constantes.Items + '.' + Constantes.Producto
            });
            if (ingresoProducto == null)
            {
                return NotFound();
            }

            try
            {
                foreach (var item in ingresoProducto.Items)
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
                await RepositorioItem.EliminarAsync(ingresoProducto.Items.ToArray(), false);
                await Repositorio.EliminarAsync(ingresoProducto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(true);
        }

        //[HttpPost("AddItem")]
        //public async Task<IActionResult> PostItem([FromBody] ItemIngresoProducto item)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    try
        //    {
        //        await RepositorioItem.RegistrarAsync(item, new object[] { item.IngresoProducto, item.Producto, item.Unidad });
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
        //        await RepositorioItem.EliminarAsync(new ItemIngresoProducto[] { item });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }

        //    return Ok(true);
        //}
    }
}