using EmergencyRoom.Core.Models;
using EmergencyRoom.Core.Service;
using EmergencyRoom.Service.Service;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EmergencyRoom.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientAttributeController //: ControllerBase
    {

        private readonly IClientAttributeService _clientAttributeService;

        public ClientAttributeController(IClientAttributeService clientAttributeService)
        {
            _clientAttributeService = clientAttributeService;
        }





        // GET: api/<ClientAttributeController>
        [HttpGet]
        public IEnumerable<ClientAttribute> Get()
        {
            return _clientAttributeService.GetAll();
        }

        // GET api/<ClientAttributeController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ClientAttributeController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ClientAttributeController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ClientAttributeController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
