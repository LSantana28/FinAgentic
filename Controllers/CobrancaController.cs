using CobrAI.Repositories;
using CobrAI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CobrAI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CobrancaController : ControllerBase
    {
        private readonly CobrancaService _cobrancaService;

        public CobrancaController(CobrancaService cobrancaService)
        {
            _cobrancaService = cobrancaService;
        }

        [HttpGet("faturas-vencidas")]
        public async Task<IActionResult> BuscarFaturasVencidas()
        {
            var faturas = await _cobrancaService.BuscarFaturasVencidas();

            return Ok(faturas);
        }

        [HttpGet("preparar")]
        public async Task<IActionResult> PrepararCobrancas()
        {
            var cobrancas = await _cobrancaService.PrepararCobrancas();

            return Ok(cobrancas);
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarCobrancas()
        {
            await _cobrancaService.RegistrarCobrancas();

            return Ok("Cobranças registradas com sucesso.");
        }
    }
}