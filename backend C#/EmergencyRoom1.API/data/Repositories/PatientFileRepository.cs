    using core.Models;
    using core.Repositories;
    using Microsoft.EntityFrameworkCore;
 

    namespace data.Repositories
    {
        public class PatientFileRepository:IPatientFileRepository
        {


            private readonly DataContext _context;

            public PatientFileRepository(DataContext context)
            {
                _context = context;
            }

        public List<PatientFile> GetList()
        {
            return _context.patientFileList.Include(p => p.PatientAttributes).ToList(); ;

        }

            public void Add(PatientFile patientFile)
            {
                _context.patientFileList.Add(patientFile);
                _context.SaveChanges();
            }

            public PatientFile? GetById(int id)
            {
                var patientFile = _context.patientFileList.FirstOrDefault(p => p.Id == id);
                if (patientFile != null)
                {
                   // patientFile.UpdateFinalScore(); // מחשב ניקוד מעודכן
                    _context.SaveChanges();
                }
                return patientFile;
            }
            public void DeletePatientFile(int id)
            {
                PatientFile p = _context.patientFileList.Find(id);
                _context.Remove(p);
                _context.SaveChanges() ;
            }

            public void Update(PatientFile patientFile)
            {
                _context.patientFileList.Update(patientFile);
                _context.SaveChanges();
            }
            public List<PatientFile> GetFilesByPatientId(int patientId)
            {
                return _context.patientFileList
                    .Where(pf => pf.PatientId == patientId)
                    .Include(p => p.PatientAttributes)
                    .ToList();
            }

            public void UpdatePatientFileScore(int patientFileId, double finalScore)
            {
                var patientFile = _context.patientFileList.FirstOrDefault(pf => pf.Id == patientFileId);
                if (patientFile != null)
                {
                    patientFile.FinalScore = finalScore;
                    _context.SaveChanges();
                }
            }
            public PatientFile GetByPatientId(int patientId)
            {
                return _context.patientFileList.FirstOrDefault(pf => pf.PatientId == patientId);
            }

        }


    }


