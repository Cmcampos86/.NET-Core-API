using Microsoft.AspNetCore.Mvc;

namespace PrimeiraAPI.Controllers
{
    [ApiController]
    [Route("api/demo")]
    public class TesteController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new Produto { Id = 1, Nome = "Teste"});
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            return Ok(new Produto { Id = 1, Nome = "Teste" }); //200
        }

        [HttpPost]
        public IActionResult Post(Produto produto)
        {
            return CreatedAtAction("Get", new { id = produto.Id }, produto); //201
        }

        [HttpPut("{id:int}")]
        public IActionResult Put(int id, Produto produto)
        {
            if (id != produto.Id) return BadRequest();

            return NoContent(); //204
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            return NoContent();
        }
    }
}
