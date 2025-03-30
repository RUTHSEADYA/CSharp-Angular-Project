using core.Models;
using core.Service;
using Microsoft.AspNetCore.Mvc;
using service.Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace apiProject.Controllers
{
    [Route("api/queue/")]
    [ApiController]
    public class QueueController : ControllerBase


    {
        private readonly IQueueManagerService _queueManagerService;


        public QueueController(IQueueManagerService queueManagerService)
        {
            _queueManagerService = queueManagerService;
        }

        [HttpGet("next")]
        public IActionResult GetNextPatient()
        {
            var patient = _queueManagerService.GetNextPatient();
            if (patient == null)
                return NotFound(new { message = "אין מטופלים בתור" });


            return Ok(patient);
        }

        [HttpPost("add")]
        public IActionResult AddPatientToQueue([FromBody] PatientFile patientFile)
        {
            _queueManagerService.AddPatientToQueue(patientFile);
            return Ok("המטופל נוסף בהצלחה");
        }

        [HttpPost("start-treatment/{id}")]
        public IActionResult StartTreatment(int id)
        {
            _queueManagerService.StartTreatment(id);
            return Ok(new { message = "הטיפול החל" });
        }

        [HttpPost("end-treatment/{id}")]
        public IActionResult EndTreatment(int id)
        {
            _queueManagerService.EndTreatment(id);
            return Ok(new { message = "הטיפול הסתיים" });
        }

        [HttpGet("all-by-score")]
        public IActionResult GetAllPatientsByScore()
        {
            var patients = _queueManagerService.GetAllPatientsByScore();
            return Ok(patients);
        }

        [HttpGet("size")]
        public ActionResult<int> GetQueueSize()
        {
            int queueSize = _queueManagerService.GetQueueSize();
            return Ok(queueSize);
        }
        [HttpGet("getPatientInTreatment")]
        public ActionResult<int> GetPatientInTreatment()
        {
            int queueSize = _queueManagerService.GetNumOfPatientInTreatment();
            return Ok(queueSize);
        }
        [HttpGet("avarage")]
        public ActionResult<double> GetQueueAvarage()
        {
            double queueSize = _queueManagerService.GetQueueSize()*10;
            return Ok(queueSize);
        }
     


    }
}
