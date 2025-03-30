using core.Models;

namespace core.Repositories
{
    public interface IQueueManagerRepository
    {

         void UpdateQueue();
         PatientFileQueueDto? GetNextPatient();
         void AddPatientToQueue(PatientFile patientFile);
         void StartTreatment(int patientId);
         void EndTreatment(int patientId);
         public void StartAutoUpdate();
         List<PatientFileQueueDto> GetAllPatientsByScore();
         int GetQueueSize();
         int GetNumOfPatientInTreatment();




    }
}
