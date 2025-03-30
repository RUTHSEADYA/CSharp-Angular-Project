using core.Models;
using core.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;



namespace apiProject.Controllers
{
    [Route("api/patientFile/")]
    [ApiController]
    public class PatientFileController : ControllerBase
    {

        private readonly IPatientFileService _patientFileService;
        private readonly IPatientService _patientService;


        public PatientFileController(IPatientFileService patientFileService, IPatientService patientService)
        {
            _patientFileService = patientFileService;
            _patientService = patientService;
        }

        [HttpGet()]
        public IEnumerable<PatientFile> Get()
        {
            return _patientFileService.GetAll();
        }


        [HttpGet("getPatientFileById/{id:int}")]
        public ActionResult<PatientFile> GetById(int id)
        {
            var patientFile = _patientFileService.GetById(id);
            if (patientFile == null)
            {
                return NotFound("הטופס לא נמצא");
            }
            return Ok(patientFile);
        }
       
        [HttpPost("addPatientFile")]
        public async Task<IActionResult> AddPatientFile([FromBody] PatientFile newFile)
        {
            if (newFile == null || newFile.PatientId == 0)
            {
                return BadRequest("Invalid patient file data.");
            }

            var patient = _patientService.GetById(newFile.PatientId);
            if (patient == null)
            {
                return NotFound("מטופל לא נמצא");
            }

            var patientFile = new PatientFile
            {
                Status = newFile.Status,
                EntryTime = DateTime.Now,
                PatientId = newFile.PatientId,
                Descreption = newFile.Descreption,
                PatientAttributes = newFile.PatientAttributes,
            };

            _patientFileService.AddPatientFile(patientFile);
            Console.WriteLine("Received request to add patient file");
            Console.WriteLine(JsonConvert.SerializeObject(newFile));


            return Ok(new { message = "טופס נוסף בהצלחה!", patientFileId = patientFile.Id });
        }



        [HttpDelete("{id}")]
        public IActionResult DeletePatientFile(int id)
        {
            var patientFile = _patientFileService.GetById(id);
            if (patientFile == null)
            {
                return NotFound("טופס לא נמצא");
            }

            _patientFileService.DeletePatientFile(id);
            return Ok("הטופס נמחק בהצלחה");
        }

        [HttpPost("addAttributeToPatientFile/{patientFileId}/{attributeId}")]
        public IActionResult AddAttributeToPatientFile(int patientFileId, int attributeId)
        {
            try
            {
                _patientFileService.AddAttributeToPatientFile(patientFileId, attributeId);
                return Ok("מאפיין נוסף לטופס בהצלחה!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("attributes")]
        public ActionResult<IEnumerable<ClientAttribute>> GetAttributes()
        {
            var attributes = _patientFileService.GetAllAttributes();
            return Ok(attributes);
        }




        [HttpPost("calculateScore")]
        public ActionResult<double> CalculateTotalScore([FromBody] PatientFile patientFile)
        {
            try
            {
                if (patientFile == null)
                    return BadRequest("PatientFile is null.");
                if (patientFile.PatientAttributes == null || !patientFile.PatientAttributes.Any())
                    return BadRequest("No attributes provided for score calculation.");
                if (patientFile.Id == 0)
                    return BadRequest("PatientFile ID is missing.");

                double totalScore = _patientFileService.CalculateTotalScore(patientFile);
                return Ok(totalScore);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error calculating score: {ex.Message}");
            }
        }

        [HttpPut("updateFinalScore/{id}")]
        [Consumes("application/json")]
        public IActionResult UpdateFinalScore(int id, [FromBody] PatientFileDto request)
        {
            Console.WriteLine($"🔵 בקשה התקבלה: ID = {id}, FinalScore = {request.FinalScore}");

            var patientFile = _patientFileService.GetById(id);
            if (patientFile == null)
            {
                return NotFound(new { message = "הטופס לא נמצא" });
            }

            patientFile.FinalScore = request.FinalScore;
            try
            {
                _patientFileService.Update(patientFile);
                Console.WriteLine("✅ עדכון בוצע בהצלחה");
                return Ok(new { message = "Final score updated successfully" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ שגיאה: {ex.Message}");
                return StatusCode(500, new { message = "Error updating final score", error = ex.Message });
            }
        }


    

    }
}

