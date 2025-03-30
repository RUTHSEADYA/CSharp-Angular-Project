using core.Models;


namespace core.Service
{
    public interface IPatientFileService
    {
        List<PatientFile> GetAll();
        void AddPatientFile(PatientFile patientFile);
        PatientFile GetById(int id);
        void DeletePatientFile(int id);
        void AddAttributeToPatientFile(int patientFileId, int attributeId);
        void Update(PatientFile patientFile);
        List<PatientFile> GetFilesByPatientId(int patientId);
        IEnumerable<ClientAttribute> GetAllAttributes();
        double CalculateTotalScore(PatientFile patientFile);
        PatientFile GetByPatientId(int patientId);
        public void UpdateFinalScore(int id);




    }
}
