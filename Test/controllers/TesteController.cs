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

        [HttpPost("criar/12345")]
        public IActionResult TesteBatataPotenteDemais([FromBody] Dados dados)
        {
            return CreatedAtAction(nameof(ObterDados), dados);
        }

        [HttpPost("andar/{pe:int}")]
        public IActionResult Coisa(int pe)
        {
            return NoContent();
        }

        [HttpDelete("caminho/sozinho")]
        public IActionResult Cabeca(int pe)
        {
            return NoContent();
        }
    }
}