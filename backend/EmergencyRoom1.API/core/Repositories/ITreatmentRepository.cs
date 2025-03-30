using core.Models;


namespace core.Repositories
{
    public interface ITreatmentRepository
    {
      
        Task<Patient> GetByIdAsync(int patientId);
        Task AddTreatment(Treatment treatment);



    }
}
