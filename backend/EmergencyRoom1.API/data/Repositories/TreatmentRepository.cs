using core.Models;
using core.Repositories;
using Microsoft.EntityFrameworkCore;


namespace data.Repositories
{
    public class TreatmentRepository:ITreatmentRepository
    {

        private readonly DataContext _context;

        public TreatmentRepository(DataContext context)
        {
            _context = context;
        }
        public async Task AddTreatment(Treatment treatment)
        {
            await _context.Treatments.AddAsync(treatment);
            await _context.SaveChangesAsync();
        }

        public async Task<Patient> GetByIdAsync(int patientId)
        {
            return await _context.patientList
                                 .Include(p => p.Treatment)
                                 .FirstOrDefaultAsync(p => p.Id == patientId);
        }


    }
}
