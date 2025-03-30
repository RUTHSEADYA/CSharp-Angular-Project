using core.Models;
using core.Repositories;
using Microsoft.EntityFrameworkCore;


namespace data.Repositories
{
    public class QueueManagerRepository:IQueueManagerRepository
    {
        private readonly DataContext _context;

        public QueueManagerRepository(DataContext context) { _context = context;}

        public void UpdateQueue()
        {
            var patientFiles = _context.patientFileList.Where(p => p.Status == "Waiting").ToList();

            foreach (var file in patientFiles)
            {
                file.UpdateFinalScore();
            }

            _context.SaveChanges();
        }

        public PatientFileQueueDto? GetNextPatient()
        {
            var patientFile = _context.patientFileList
                .Include(p => p.Patient)
                .Where(p => p.Status == "Waiting")
                .OrderByDescending(p => p.FinalScore)
                .FirstOrDefault();

            if (patientFile == null)
                return null;

            return new PatientFileQueueDto
            {
                Id = patientFile.Id,
                EntryTime = patientFile.EntryTime,
                Descreption = patientFile.Descreption,
                Status = patientFile.Status,
                PatientId = patientFile.PatientId,
                FinalScore = patientFile.FinalScore,
                Patient = new PatientDto
                {
                    Id = patientFile.Patient.Id,
                   PatientIdentity=patientFile.Patient.PatientIdentity,
                   Name = patientFile.Patient.Name
                }
            };
        }

        public void AddPatientToQueue(PatientFile patientFile)
        {
            _context.patientFileList.Add(patientFile);
            _context.SaveChanges();
            UpdateQueue();
        }

        public void StartTreatment(int patientId)
        {
            var previousPatient = _context.patientFileList.FirstOrDefault(p => p.Status == "In Treatment");

            if (previousPatient != null)
            {
                previousPatient.Status = "Treated"; 
            }

            var patientFile = _context.patientFileList.FirstOrDefault(p => p.PatientId == patientId && p.Status == "Waiting");

            if (patientFile != null)
            {
                patientFile.Status = "In Treatment"; 
            }

            _context.SaveChanges();

            var checkUpdatedPatient = _context.patientFileList.FirstOrDefault(p => p.PatientId == patientId);
            Console.WriteLine($"מטופל {checkUpdatedPatient?.PatientId} סטטוס: {checkUpdatedPatient?.Status}");
        }

        public void EndTreatment(int patientId)
        {
            var patient = _context.patientFileList.FirstOrDefault(p => p.PatientId == patientId);
            if (patient != null)
            {
                patient.Status = "Treated";
                _context.SaveChanges(); 
            }
         
        }


        public List<PatientFileQueueDto> GetAllPatientsByScore()
        {
            return _context.patientFileList
                .Include(p => p.Patient)
                .Where(p => p.Status == "Waiting")
                .OrderByDescending(p => p.FinalScore)
                .Select(patientFile => new PatientFileQueueDto
                {
                    Id = patientFile.Id,
                    EntryTime = patientFile.EntryTime,
                    Descreption = patientFile.Descreption,
                    Status = patientFile.Status,
                    PatientId = patientFile.PatientId,
                    FinalScore = patientFile.FinalScore,
                    Patient = new PatientDto
                    {
                        Id = patientFile.Patient.Id,
                        PatientIdentity = patientFile.Patient.PatientIdentity,
                        Name = patientFile.Patient.Name
                    }
                })
                .ToList();
        }

        public int GetQueueSize()
        {
            Console.WriteLine("hhhhhhhhhhhhhhh" + _context.patientFileList.Count(p => p.Status == "Waiting"));
            return _context.patientFileList.Count(p => p.Status == "Waiting"||p.Status=="In Treatment");
        }
        public int GetNumOfPatientInTreatment()
        {
            PatientFile patientFile = _context.patientFileList.FirstOrDefault(p => p.Status == "In Treatment");
            if (patientFile != null) { 
            return patientFile.PatientId;
        }
            return 0;
        }
        public void StartAutoUpdate()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    UpdateQueue();
                    await Task.Delay(30000); 
                }
            });
        }

    }
}
