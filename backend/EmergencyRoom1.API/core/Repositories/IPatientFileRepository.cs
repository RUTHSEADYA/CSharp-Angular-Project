using core.Models;


namespace core.Repositories
{
    public interface IPatientFileRepository
    {
        List<PatientFile> GetList();
        void Add(PatientFile patientFile);
        PatientFile GetById(int id); 
        void DeletePatientFile(int id);
        void Update(PatientFile patientFile);
        List<PatientFile> GetFilesByPatientId(int patientId);
        void UpdatePatientFileScore(int patientFileId, double totalScore);
        PatientFile GetByPatientId(int patientId);


    }
    
}
