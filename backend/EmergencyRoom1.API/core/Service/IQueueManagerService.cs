using core.Models;


namespace core.Service
{
    public interface IQueueManagerService
    {

         void UpdateQueue();
         PatientFileQueueDto? GetNextPatient();
         void AddPatientToQueue(PatientFile patientFile);
         void StartTreatment(int patientId);
         void EndTreatment(int patientId);
         void StartAutoUpdate();
         List<PatientFileQueueDto> GetAllPatientsByScore();
         int GetQueueSize();
         int GetNumOfPatientInTreatment();


    }
}
