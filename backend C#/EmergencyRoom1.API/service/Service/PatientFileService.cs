using core.Models;
using core.Repositories;
using core.Service;
using data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace service.Service
{
    public class PatientFileService:IPatientFileService
    {


        private readonly IPatientFileRepository _patientFileReposirory;
        private readonly IPatientRepository _patientRepository;
        private readonly IClientAttributeRepository _clientAttributeReposirory;

        public PatientFileService(IPatientFileRepository patientFileRepository, IPatientRepository patientRepository, IClientAttributeRepository clientAttributeReposirory)
        {
            _patientFileReposirory = patientFileRepository;
            _patientRepository = patientRepository;
            _clientAttributeReposirory = clientAttributeReposirory;
        }

        public List<PatientFile> GetAll()
        {
            return _patientFileReposirory.GetList();

        }

        //public void AddPatientFile(PatientFile patientFile)
        //{
        //    _patientFileReposirory.Add(patientFile);
        //}
        public void AddPatientFile(PatientFile patientFile)
        {
            // חיפוש המטופל לפי PatientId
            var patient = _patientRepository.GetById(patientFile.PatientId);

            if (patient != null)
            {
                // הוספת הטופס לרשימת הטפסים של המטופל
                patient.Files.Add(patientFile);  // זהו קשר של One-to-Many, אנחנו מוסיפים את הטופס לרשימה של הטפסים

                // שמירה של הטופס ב-Repository של טפסים
                _patientFileReposirory.Add(patientFile);
            }
            else
            {
                throw new Exception("הפציינט לא נמצא");
            }
        }
        public PatientFile GetById(int id) // ✅ מימוש הפונקציה
        {
            return _patientFileReposirory.GetById(id);
        }
        public PatientFile GetByPatientId(int patientId)
        {
            return _patientFileReposirory.GetByPatientId(patientId);
        }

        public void DeletePatientFile(int id)
        {
            _patientFileReposirory.DeletePatientFile(id);
        }
        public void Update(PatientFile patientFile)
        {
            _patientFileReposirory.Update(patientFile);
        }

        public void AddAttributeToPatientFile(int patientFileId, int attributeId)
        {
            // שליפת הטופס מה- Repository
            var patientFile = _patientFileReposirory.GetById(patientFileId);
            if (patientFile == null)
                throw new Exception("טופס לא נמצא");

            // שליפת המאפיין מה- Repository (צריך להוסיף ClientAttributeRepository אם אין)
            var attribute = _clientAttributeReposirory.GetById(attributeId);
            if (attribute == null)
                throw new Exception("מאפיין לא נמצא");

            // הוספת המאפיין לרשימה של הטופס
            patientFile.PatientAttributes.Add(attribute);

            // עדכון הטופס ב- Repository
            _patientFileReposirory.Update(patientFile);
        }
        public List<PatientFile> GetFilesByPatientId(int patientId)
        {
            return _patientFileReposirory.GetFilesByPatientId(patientId);
        }
        public IEnumerable<ClientAttribute> GetAllAttributes()
        {
            return _clientAttributeReposirory.GetList();
        }

        //public double CalculateTotalScore(int patientId)
        //{
        //    // שליפת כל הטפסים של המטופל לפי ה-ID
        //    var patientFiles = _patientFileReposirory.GetFilesByPatientId(patientId);

        //    if (patientFiles == null || !patientFiles.Any())
        //    {
        //        throw new Exception("לא נמצאו טפסים למטופל");
        //    }

        //    // חישוב סכום הניקוד מכל המאפיינים
        //    double totalScore = patientFiles
        //        .SelectMany(pf => pf.PatientAttributes) // איחוד כל המאפיינים מכל הטפסים
        //        .Sum(attr => attr.Score); // סכימת הניקוד של כל המאפיינים

        //    return totalScore;
        //}

        //public double CalculateTotalScore(PatientFile patientFile)
        //{
        //    if (patientFile.PatientAttributes == null || !patientFile.PatientAttributes.Any())
        //    {
        //        throw new Exception("לא נמצאו מאפיינים לחישוב ניקוד");
        //    }

        //    // חישוב סכום הניקוד מכל המאפיינים שנשלחו בבקשה
        //    double totalScore = patientFile.PatientAttributes.Sum(attr => attr.Score); // לקיחה ישירה של הניקוד

        //    return totalScore;
        //}

        public double CalculateTotalScore(PatientFile patientFile)
        {
            if (patientFile == null || patientFile.PatientAttributes == null || !patientFile.PatientAttributes.Any())
            {
                throw new Exception("לא נמצאו מאפיינים לחישוב ניקוד");
            }

            double totalScore = patientFile.PatientAttributes.Sum(attr => attr.Score);

            // עדכון הניקוד במסד הנתונים
            _patientFileReposirory.UpdatePatientFileScore(patientFile.Id, totalScore);

            return totalScore;
        }



        private double GetAttributeScore(string attributeName)
        {
            var attribute = _clientAttributeReposirory.GetAttributeByName(attributeName);
            return attribute?.Score ?? 0;
        }

        public void UpdateFinalScore(int id)
        {
            var patientFile = _patientFileReposirory.GetById(id);
            if (patientFile != null)
            {
                patientFile.UpdateFinalScore();
                _patientFileReposirory.Update(patientFile);
            }
        }

    }
}
