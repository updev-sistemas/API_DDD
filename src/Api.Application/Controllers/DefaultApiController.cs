using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace application.Controllers
{ 
    [ApiController]
    [Route("[controller]")]
    public class DefaultApiController : ControllerBase
    {
        private readonly ILogger<DefaultApiController> _logger;

        public DefaultApiController(ILogger<DefaultApiController> logger) => _logger = logger;

        [HttpGet]
        public IEnumerable<object> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new
            {
                Date = DateTime.Now.AddDays(index),
                Summary = $"Dados {index}"
            })
            .ToArray();
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(new
            {
                Date = DateTime.Now.AddDays(id),
                Summary = $"Você selecionou {id}"
            });
        }
    }
}
