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

        [HttpPost("criar")]
        public IActionResult CriarDados([FromBody] Dados dados)
        {
            return CreatedAtAction(nameof(ObterDados), dados);
        }

        [HttpDelete("andar/{pe:int}")]
        public IActionResult DeletarDados(int pe)
        {
            return NoContent();
        }
    }
}