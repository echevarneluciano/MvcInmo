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
    public class PagosController : Controller
    {
        private readonly DataContext contexto;

        public PagosController(DataContext contexto)
        {
            this.contexto = contexto;
        }

        // GET api/<controller>/5
        [HttpPost("contrato")]
        public async Task<IActionResult> porContrato([FromBody] ContratoApi contrato)
        {
            try
            {
                var idContrato = contrato.Id;
                return Ok(contexto.PagosApis.Include(e => e.contrato).Where(e => e.ContratoId == idContrato).Include(e => e.contrato.Inmueble));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}