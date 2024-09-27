using Microsoft.AspNetCore.Mvc;

namespace docsnapAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeuController : ControllerBase
    {
        [HttpGet]
        public IActionResult ObterDados()
        {
            return Ok(new { mensagem = "Olá, mundo!" });
        }

        [HttpPost]
        public IActionResult CriarDados([FromBody] Dados dados)
        {
            return CreatedAtAction(nameof(ObterDados), dados);
        }

        [HttpPut]
        public IActionResult AtualizarDados([FromBody] Dados dados)
        {
            return Ok(dados);
        }

        [HttpDelete]
        public IActionResult DeletarDados()
        {
            return NoContent();
        }
    }

    public class Dados
    {
        public string? Nome { get; set; }
        public int Idade { get; set; }
    }
}