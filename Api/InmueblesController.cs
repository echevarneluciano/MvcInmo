using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using MvcInmo.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MvcInmo.Api
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class InmueblesController : Controller
    {
        private readonly DataContext contexto;
        public InmueblesController(DataContext contexto)
        {
            this.contexto = contexto;
        }

        // GET: api/<controller>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var usuario = User.Identity.Name;
                return Ok(contexto.Inmuebles.Include(e => e.Duenio).Where(e => e.Duenio.Email == usuario));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("actualizar")] ///// este es el metodo para actualizar
        public async Task<IActionResult> Actualizar([FromBody] Inmueble inmueble)
        {
            try
            {
                var usuario = User.Identity.Name;
                var entidad = await contexto.Inmuebles.Include(e => e.Duenio).Where(e => e.Duenio.Email == usuario).SingleOrDefaultAsync(x => x.Id == inmueble.Id);
                if (entidad != null)
                {
                    entidad.Estado = inmueble.Estado == 1 ? 0 : 1;
                    await contexto.SaveChangesAsync();
                    return Ok(entidad);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }

}