using core.Models;
using Microsoft.AspNetCore.Mvc;
using service.Service;


using core.Service;

namespace apiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TreatmentController : ControllerBase
    {

        private readonly EmailService _emailService;
        private readonly PdfService _pdfService;
        private readonly ITreatmentService _treatmentService;


        public TreatmentController(ITreatmentService treatmentService, EmailService emailService, PdfService pdfService)
        {
            _treatmentService = treatmentService;
            _emailService = emailService;
            _pdfService = pdfService;
        }

       
        [HttpPost("summaryTreatment/{patientId}")]
        public async Task<IActionResult> EndTreatment(int patientId, [FromBody] Treatment treatmentData)
        {
            var patient = await _treatmentService.GetByIdAsync(patientId);

            if (patient == null)
            {
                return NotFound("Patient not found");
            }

            if (patient.Treatment == null)
            {
                patient.Treatment = new Treatment();
            }

            patient.Treatment.Diagnosis = treatmentData.Diagnosis;
            patient.Treatment.Procedure = treatmentData.Procedure;
            patient.Treatment.Recommendations = treatmentData.Recommendations;

            string emailBody = $@"
        <h2>סיכום טיפול רפואי</h2>
        <p><strong>שם מטופל:</strong> {patient.Name}</p>
        <p><strong>אבחון:</strong> {patient.Treatment.Diagnosis}</p>
        <p><strong>טיפול שבוצע:</strong> {patient.Treatment.Procedure}</p>
        <p><strong>המלצות:</strong> {patient.Treatment.Recommendations}</p>
        <p><strong>תודה רבה ובריאות שלמה!</strong></p>
    ";

            // יצירת PDF
            var pdfBytes = _pdfService.GenerateTreatmentSummaryPdf(
           patientName: patient.Name,
           patientIdentity: patient.PatientIdentity,
           birthDate: patient.DateOfBirth, 
            diagnosis: patient.Treatment.Diagnosis,
            procedure: patient.Treatment.Procedure,
             recommendations: patient.Treatment.Recommendations
            );

          
            await _emailService.SendEmailWithAttachmentAsync(
                patient.Email,
                "TreatmentSummary.pdf", 
                pdfBytes
            );

            return Ok(new { message = "Treatment updated and email sent successfully" });
        }


        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{id}")]
        public async Task <Patient> GetById(int patientId)
        {
           return await _treatmentService.GetByIdAsync( patientId );
        }

       
    }
}

