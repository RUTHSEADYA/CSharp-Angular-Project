using core.Models;
using core.Repositories;
using core.Service;


namespace service.Service
{
    public class QueueManagerService : IQueueManagerService
    {

        private readonly IQueueManagerRepository _queueManagerReposirory;


        public QueueManagerService(IQueueManagerRepository queueManagerReposirory)
        {
            _queueManagerReposirory = queueManagerReposirory;

        }

        public void UpdateQueue()
        {
            _queueManagerReposirory.UpdateQueue();
        }

        public PatientFileQueueDto? GetNextPatient()
        {
            return _queueManagerReposirory.GetNextPatient();
        }

        public void AddPatientToQueue(PatientFile patientFile)
        {
            _queueManagerReposirory.AddPatientToQueue(patientFile);
        }

        public void StartTreatment(int patientId)
        {
            _queueManagerReposirory.StartTreatment(patientId);
        }

        public void EndTreatment(int patientId)
        {
            _queueManagerReposirory.EndTreatment(patientId);
        }

        public void StartAutoUpdate()
        {
            _queueManagerReposirory.StartAutoUpdate();
        }

        public List<PatientFileQueueDto> GetAllPatientsByScore()
        {
            return _queueManagerReposirory.GetAllPatientsByScore();
        }
        public int GetQueueSize()
        {
            return _queueManagerReposirory.GetQueueSize();
        }
        public int GetNumOfPatientInTreatment()
        {
            return _queueManagerReposirory.GetNumOfPatientInTreatment();
        }

    }
}
