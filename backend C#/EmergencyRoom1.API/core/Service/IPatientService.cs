using core.Models;


namespace core.Service
{
    public interface IPatientService
    {

        List<Patient> GetAll();
        void AddPatient(Patient patient);
        Patient GetById(int id);
        void DeletePatient(int id);
        Patient? GetByIdentity(string identity);
        void DeletePatientByNumberID(string identity);
        void UpdatePatient(Patient patient);



    }
}
