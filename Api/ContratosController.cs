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
    public class ContratosController : Controller
    {
        private readonly DataContext contexto;

        public ContratosController(DataContext contexto)
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
                return Ok(contexto.ContratosApis.Include(e => e.Inmueble).ThenInclude(e => e.Duenio).Where(e => (e.Inmueble.Duenio.Email == usuario) && (e.FechaFin >= DateTime.Now)).Include(e => e.Inquilino));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}