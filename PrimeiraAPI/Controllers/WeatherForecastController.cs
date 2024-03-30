using Microsoft.AspNetCore.Mvc;

namespace PrimeiraAPI.Controllers
{
    [ApiController] //Tem que ter esse atributo para indicar que � uma controller de api
    [Route("api/minha-controller")]
    public class WeatherForecastController : ControllerBase
    {
        public WeatherForecastController()
        {

        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult Get3()
        {
            return Ok();
        }

        [HttpGet("{id:int}/dado/{id2:int}")]
        public IActionResult Get2(int id, int id2)
        {
            return Ok();
        }
    }
}
