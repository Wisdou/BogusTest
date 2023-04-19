using Microsoft.AspNetCore.Mvc;
using TestBogus.Models;

namespace TestBogus.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetUsers")]
        public IActionResult Get()
        {
            var userFaker = UserGenerator.GetGameUserFaker();
            return Ok(userFaker.Generate(100).Dump());
        }
    }
}