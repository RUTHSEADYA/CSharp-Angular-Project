using core.Models;
using core.Service;
using iText.Commons.Actions.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using service.Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace apiProject.Controllers
{
    [Route("api/patient/")]
    [ApiController]
    public class PatientController : ControllerBase
    {


        private readonly IPatientService _patientService;
        private readonly IPatientFileService _patientFileService;
        private readonly IClientAttributeService _clientAttributeService;// הוספת PatientFileService
        private readonly ITreatmentService _treatmentService;
        public PatientController(IPatientService patientService, IPatientFileService patientFileService, IClientAttributeService clientAttributeService, ITreatmentService treatmentService)
        {
            _patientService = patientService;
            _patientFileService = patientFileService;
            this._clientAttributeService = clientAttributeService;
            _treatmentService = treatmentService;
        }






        // GET: api/<PatientController>
        [HttpGet("getAllPatints")]
        public IEnumerable<Patient> Get()
        {
            return _patientService.GetAll();
        }
        [HttpGet("{id}")]
        public ActionResult<Patient> GetPatientById(int id)
        {
            var patient = _patientService.GetById(id); // משתמשים בשירות בשביל לקבל מטופל לפי ID

            if (patient == null)
            {
                return NotFound("מטופל לא נמצא");
            }

            return Ok(patient);
        }



        [HttpPost("addPatient")]
        public async Task<IActionResult> AddPatient([FromBody] Patient newPatient)
        {
            var p = _patientService.GetByIdentity(newPatient.PatientIdentity);
            if (p != null)
            {
                return BadRequest(new { message = ("The ID number already exist please search") });

            }
            if (newPatient == null)
            {
                return BadRequest("Invalid patient data.");
            }

            var patient = new Patient
            {
                PatientIdentity = newPatient.PatientIdentity,
                Name = newPatient.Name,
                Gender = newPatient.Gender,
                DateOfBirth= newPatient.DateOfBirth,
                Address = newPatient.Address,
                Phone = newPatient.Phone,
                Email = newPatient.Email,
                EntryTime = DateTime.Now,
                Files = new List<PatientFile>()
            };

            if (newPatient.Files != null)
            {
                foreach (var file in newPatient.Files)
                {
                    patient.Files.Add(new PatientFile
                    {
                        Status = file.Status,
                        EntryTime = DateTime.Now
                    });
                }
            }

            _patientService.AddPatient(patient);
            return Ok(new { message = "Patient added successfully!", patientId = patient.Id });
        }

   
        [HttpPost("addPatientFile/{patientId}")]
        public async Task<IActionResult> AddPatientFile(int patientId, [FromBody] PatientFile file)
        {
            var patient = _patientService.GetById(patientId);
            if (patient == null)
            {
                return NotFound("Patient not found.");
            }

            var attributes = new List<ClientAttribute>();

            if (file.PatientAttributes != null && file.PatientAttributes.Any())
            {
                foreach (var attr in file.PatientAttributes)
                {
                    var existingAttribute = _clientAttributeService.GetById(attr.Id); // שימוש בשירות במקום גישה ישירה ל-DB

                    if (existingAttribute != null)
                    {
                        attributes.Add(existingAttribute); // משתמשים במאפיין קיים
                    }
                    else
                    {
                        var newAttribute = new ClientAttribute
                        {
                            AttributeName = attr.AttributeName ?? "Unknown",
                            Score = attr.Score
                        };

                        _clientAttributeService.AddClientAttribute(newAttribute); // שמירה למסד הנתונים דרך ה-Service
                        attributes.Add(newAttribute);
                    }
                }
            }

            var newFile = new PatientFile
            {
                Status = file.Status,
                EntryTime = DateTime.Now,
                PatientId = patientId,
                Descreption = file.Descreption,
                PatientAttributes = attributes // שמירת אובייקטי `ClientAttribute` ולא רק מזהים
            };

            _patientFileService.AddPatientFile(newFile);
            return Ok(new { message = "File added successfully!", newFileId = newFile.Id });
        }



            // PUT api/<PatientController>/5
            [HttpPut("updatePatient/{id}")]
            public IActionResult Put(int id, [FromBody] Patient patient)
            {
             var p=_patientService.GetById(id);
                if(p!=null) { 
                    p.Name = patient.Name;
                    p.Address = patient.Address;
                    p.PatientIdentity = patient.PatientIdentity;
                    p.EntryTime= DateTime.Now;
                    p.Email= patient.Email;
                    p.Gender = patient.Gender;
                    p.DateOfBirth=patient.DateOfBirth;
                  p.Phone = patient.Phone;
                  _patientService.UpdatePatient(p);
                return Ok( new { message = "the patient updated successfuly", patient = p });

        }
                return BadRequest();

    }



    // DELETE api/<PatientController>/5
    [HttpDelete("delete/{id}")]
        public IActionResult DeletePatient(int id)
        {
            var p = _patientService.GetById(id);
            if (p == null)
            {
                return NotFound("the patient not found");
            }
            _patientService.DeletePatient(id);
            return Ok("the patient deleted successfuly");
        }

        [HttpDelete("deleteByNumberId/{identity}")]
        public IActionResult DeletePatientByNumberId(string identity)
        {
            var p = _patientService.GetByIdentity(identity);
            if (p == null)
            {
                return NotFound("the patient not found");
            }
            _patientService.DeletePatientByNumberID(identity);
            return Ok(new { message = "the patient deleted successfuly" });
        }



        [HttpGet("searchByIdentity/{identity}")]
        public IActionResult GetPatientByIdentity(string identity)
        {
            var patient = _patientService.GetByIdentity(identity);

            if (patient == null)
            {
                return NotFound("מטופל לא נמצא");
            }

            Console.WriteLine($"🔎 כמות טפסים: {patient.Files?.Count}");

            return Ok(new
            {
                PatientId = patient.Id,
                PatientIdentity = patient.PatientIdentity,
                Name = patient.Name,
                Gender = patient.Gender,
                Address = patient.Address,
                Phone = patient.Phone,
                Email=patient.Email,
                EntryTime = patient.EntryTime,
                Files = patient.Files?.Select(f => new
                {
                    FileId = f.Id,
                    Status = f.Status,
                    EntryTime = f.EntryTime,
                    Description = f.Descreption, // אם תיקנת את השם, שנה כאן גם ל- Description
                    Attributes = f.PatientAttributes?.Select(attr => new
                    {
                        AttributeId = attr.Id,
                        AttributeName = attr.AttributeName,
                        Score = attr.Score
                    }).ToList()
                }).ToList()
            });
        }

        [HttpPost("addTreatment/{patientId}")]
        public async Task<IActionResult> AddTreatment(int patientId, [FromBody] Treatment treatment)
        {
            // חיפוש המטופל במסד הנתונים
            var patient = _patientService.GetById(patientId);

            if (patient == null)
            {
                return NotFound("Patient not found");
            }

            // יצירת טיפול חדש
            var newTreatment = new Treatment
            {
                Diagnosis = treatment.Diagnosis,
                Procedure = treatment.Procedure,
                Recommendations = treatment.Recommendations
            };

            patient.Treatment = newTreatment;

            _treatmentService.AddTreatment(newTreatment);

            return Ok(new { message = "Treatment added successfully" });
        }


    }
}
