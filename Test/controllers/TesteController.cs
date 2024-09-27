using Microsoft.AspNetCore.Mvc;

namespace docsnapAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TesteController : ControllerBase
    {
        [HttpGet]
        public IActionResult ObterDados()
        {
            return Ok(new { mensagem = "Olá, mundo!" });
        }

        [HttpDelete]
        public IActionResult DeletarDados()
        {
            return NoContent();
        }
    }
}