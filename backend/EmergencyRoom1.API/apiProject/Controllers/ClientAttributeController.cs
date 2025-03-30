using core.Models;
using core.Service;
using service.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;


namespace EmergencyRoom.API.Controller
{
    [Route("api/clientAttribute/")]
    [ApiController]
    public class ClientAttributeController : ControllerBase
    {

        private readonly IClientAttributeService _clientAttributeService;

        public ClientAttributeController(IClientAttributeService clientAttributeService)
        {
            _clientAttributeService = clientAttributeService;
        }



        [HttpGet]
        public IEnumerable<ClientAttribute> Get()
        {
            return _clientAttributeService.GetAll();
        }

        [HttpGet("{id}")]
        public ActionResult<ClientAttribute> GetById(int id)
        {

            var attribute = _clientAttributeService.GetById(id);
            if (attribute == null)
            {
                return NotFound();
            }
            return Ok(attribute);



        }
      

        [HttpPost("addClientAttribute")]
        public IActionResult AddClientAttribute([FromBody] ClientAttribute newClientAttribute)
        {
            try
            {
                var clientAttribute = new ClientAttribute
                {
                    AttributeName = newClientAttribute.AttributeName,
                    Score = newClientAttribute.Score,
                };

                _clientAttributeService.AddClientAttribute(clientAttribute);

                return Ok(new { message = "Client attribute added successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


        [HttpDelete("deleteClientAttribute/{id}")]
        public IActionResult DeleteClientAttribute(int id)
        {
            var clientAttribute= _clientAttributeService.GetById(id);
            if(clientAttribute == null)
            {
                return NotFound(new { message="clientAttribute not found" });
            }

            _clientAttributeService.DeleteClientAttribute(id);
            return Ok(new { message = "the ClientAttribute deleted succefuly" });
        }



    }
}
