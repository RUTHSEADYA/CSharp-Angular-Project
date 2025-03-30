using core.Models;
using core.Repositories;
using core.Service;


namespace service.Service
{
    public class TreatmentService:ITreatmentService
    {

        private readonly ITreatmentRepository _treatmentRepository;


        public TreatmentService(ITreatmentRepository treatmentRepository)
        {
            _treatmentRepository=treatmentRepository;
        }

        public async Task AddTreatment(Treatment treatment)
        {
            await _treatmentRepository.AddTreatment(treatment);

        }
        public async Task<Patient> GetByIdAsync(int patientId)
        {
            return await _treatmentRepository.GetByIdAsync(patientId);
        }


    }
}
