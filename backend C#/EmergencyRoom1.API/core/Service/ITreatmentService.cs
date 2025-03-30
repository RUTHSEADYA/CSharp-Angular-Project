using core.Models;


namespace core.Service
{
    public interface ITreatmentService
    {
       
        Task<Patient> GetByIdAsync(int patientId);

        Task AddTreatment(Treatment treatment);


    }
}
