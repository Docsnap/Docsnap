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
        public IActionResult TesteBatataPotente([FromBody] Dados dados)
        {
            return CreatedAtAction(nameof(ObterDados), dados);
        }

        [HttpDelete("andar/coisa/{pe:int}")]
        public IActionResult Coisa(int pe)
        {
            return NoContent();
        }
    }
}