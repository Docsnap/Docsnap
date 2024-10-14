using Microsoft.AspNetCore.Mvc;

namespace docsnapAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TesteController : ControllerBase
    {
        [HttpGet("/batata")]
        public IActionResult ObterDados()
        {
            return Ok(new { mensagem = "Olá, mundo!" });
        }

        [HttpPost("criar/123")]
        public IActionResult TesteBatata([FromBody] Dados dados)
        {
            return CreatedAtAction(nameof(ObterDados), dados);
        }

        [HttpDelete("andar/tururu/{pe:int}")]
        public IActionResult Teste123(int pe)
        {
            return NoContent();
        }
    }
}