using core.Models;


namespace core.Repositories
{
    public interface IPatientRepository
    {

        List<Patient> GetList();
        void Add(Patient patient);
        Patient GetById(int patientId); 
        void DeletePatient(int patientId);
        void UpdatePatient(Patient patient);
        Patient? GetByIdentity(string identity);
        void DeletePatientByNumberID(string identity);


    }
}
