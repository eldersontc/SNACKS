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
    [Route("api/Categorias")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        public IRepositorioBase<Categoria> Repositorio { get; }

        public CategoriasController(IRepositorioBase<Categoria> repositorio)
        {
            Repositorio = repositorio;
        }

        [HttpPost("GetCategorias")]
        public async Task<IActionResult> GetCategorias([FromBody] Paginacion paginacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var filtros = new List<Expression<Func<Categoria, bool>>>();

            foreach (Filtro filtro in paginacion.Filtros)
            {
                switch (filtro.K)
                {
                    case Constantes.Uno:
                        filtros.Add(x => x.Nombre.Contains(filtro.V));
                        break;
                }
            }

            var result = await Repositorio.ObtenerTodosAsync(paginacion, filtros);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoria([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoria = await Repositorio.ObtenerAsync(id);

            if (categoria == null)
            {
                return NotFound();
            }

            return Ok(categoria);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategoria([FromRoute] int id, [FromBody] Categoria categoria)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != categoria.IdCategoria)
            {
                return BadRequest();
            }

            try
            {
                await Repositorio.ActualizarAsync(categoria);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(true);
        }

        [HttpPost]
        public async Task<IActionResult> PostCategoria([FromBody] Categoria categoria)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await Repositorio.RegistrarAsync(categoria);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(true);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoria([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoria = await Repositorio.ObtenerAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }

            try
            {
                await Repositorio.EliminarAsync(new Categoria[] { categoria });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(true);
        }
    }
}