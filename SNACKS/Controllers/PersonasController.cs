using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SNACKS.Data;
using SNACKS.Models;

namespace SNACKS.Controllers
{
    [Produces("application/json")]
    [Route("api/Personas")]
    [ApiController]
    public class PersonasController : ControllerBase
    {
        public IRepositorioBase<Persona> Repositorio { get; }

        public PersonasController(IRepositorioBase<Persona> repositorio)
        {
            Repositorio = repositorio;
        }

        [HttpPost("GetPersonas")]
        public async Task<IActionResult> GetPersonas([FromBody] Paginacion paginacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var filtros = new List<Expression<Func<Persona, bool>>>();

            foreach (Filtro filtro in paginacion.Filtros)
            {
                switch (filtro.K)
                {
                    case Constantes.Uno:
                        filtros.Add(x => x.TipoPersona == filtro.N);
                        break;
                    case Constantes.Dos:
                        filtros.Add(x => x.Nombres.Contains(filtro.V));
                        break;
                    case Constantes.Tres:
                        filtros.Add(x => x.Apellidos.Contains(filtro.V));
                        break;
                    case Constantes.Cuatro:
                        filtros.Add(x => x.RazonSocial.Contains(filtro.V));
                        break;
                    case Constantes.Cinco:
                        filtros.Add(x => x.NumeroDocumento.Contains(filtro.V));
                        break;
                }
            }

            var result = await Repositorio.ObtenerTodosAsync(paginacion, filtros);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPersona([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var persona = await Repositorio.ObtenerAsync(id);

            if (persona == null)
            {
                return NotFound();
            }

            return Ok(persona);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPersona([FromRoute] int id, [FromBody] Persona persona)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != persona.IdPersona)
            {
                return BadRequest();
            }

            try
            {
                await Repositorio.ActualizarAsync(persona);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

            return Ok(true);
        }

        [HttpPost]
        public async Task<IActionResult> PostPersona([FromBody] Persona persona)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await Repositorio.RegistrarAsync(persona);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

            return Ok(true);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePersona([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var persona = await Repositorio.ObtenerAsync(id);
            if (persona == null)
            {
                return NotFound();
            }

            try
            {
                await Repositorio.EliminarAsync(persona);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
            
            return Ok(true);
        }
    }
}