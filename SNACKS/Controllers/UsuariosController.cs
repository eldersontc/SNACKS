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
    [Route("api/Usuarios")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        public IRepositorioBase<Usuario> Repositorio { get; }

        public UsuariosController(IRepositorioBase<Usuario> repositorio)
        {
            Repositorio = repositorio;
        }

        [HttpPost("GetUsuarios")]
        public async Task<IActionResult> GetUsuarios([FromBody] Paginacion paginacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var filtros = new List<Expression<Func<Usuario, bool>>>();

            foreach (Filtro filtro in paginacion.Filtros)
            {
                switch (filtro.K)
                {
                    case Constantes.Uno:
                        filtros.Add(x => x.Nombre.Contains(filtro.V));
                        break;
                }
            }

            var result = await Repositorio.ObtenerTodosAsync(paginacion, filtros, new string[] { Constantes.Persona });

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsuario([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usuario = await Repositorio.ObtenerAsync(id, new string[] { Constantes.Persona });

            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }

        [HttpGet("AuthUsuario/{nombre}/{clave}")]
        public async Task<IActionResult> AuthUsuario([FromRoute] string nombre, [FromRoute] string clave)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var filtros = new List<Expression<Func<Usuario, bool>>>() {
                (x => x.Nombre.Equals(nombre)),
                (x => x.Clave.Equals(clave))
            };

            List<Usuario> usuarios = await Repositorio.ObtenerTodosAsync(filtros, new string[] { Constantes.Persona });

            if (usuarios.Count == 0)
            {
                return NotFound();
            }

            return Ok(usuarios[0]);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario([FromRoute] int id, [FromBody] Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != usuario.IdUsuario)
            {
                return BadRequest();
            }

            try
            {
                Repositorio.AgregarReferencia(usuario.Persona);
                await Repositorio.ActualizarAsync(usuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(true);
        }

        [HttpPost]
        public async Task<IActionResult> PostUsuario([FromBody] Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Repositorio.AgregarReferencia(usuario.Persona);
                await Repositorio.RegistrarAsync(usuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(true);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usuario = await Repositorio.ObtenerAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            try
            {
                await Repositorio.EliminarAsync(new Usuario[] { usuario });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(true);
        }
    }
}