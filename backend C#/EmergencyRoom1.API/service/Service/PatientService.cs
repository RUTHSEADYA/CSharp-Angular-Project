using core.Models;
using core.Repositories;
using core.Service;


namespace service.Service
{
    public class PatientService : IPatientService
    {


        private readonly IPatientRepository _patientReposirory;
        private readonly IClientAttributeRepository _clientAttributeReposirory;
        private IPatientFileRepository _patientFileReposirory;


        public PatientService(IPatientRepository patientReposirory , IClientAttributeRepository clientAttributeReposirory, IPatientFileRepository patientFileReposirory)
        {
            _patientReposirory = patientReposirory;
            _clientAttributeReposirory = clientAttributeReposirory;
            _patientFileReposirory = patientFileReposirory;
        }

        public void AddPatient(Patient patient)
        {
            _patientReposirory.Add(patient);
        }

        public List<Patient> GetAll()
        {
            return _patientReposirory.GetList();
        }
        public Patient GetById(int id) // ✅ מימוש הפונקציה
        {
            return _patientReposirory.GetById(id);
        }
        public Patient GetByIdentity(string identity) // ✅ מימוש הפונקציה
        {
            return _patientReposirory.GetByIdentity(identity);
        }

        public void DeletePatient(int id)
        {
            _patientReposirory.DeletePatient(id);


        }

        public void UpdatePatient(Patient patient)
        {
            _patientReposirory.UpdatePatient(patient);
        }


       public void DeletePatientByNumberID(string identity)
        {
            _patientReposirory.DeletePatientByNumberID(identity);
        }



    }
}