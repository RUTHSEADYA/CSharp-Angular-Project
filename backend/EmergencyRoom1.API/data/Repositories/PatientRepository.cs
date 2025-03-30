using core.Models;
using core.Repositories;
using Microsoft.EntityFrameworkCore;


namespace data.Repositories
{
    public class PatientRepository : IPatientRepository
    {

        private readonly DataContext _context;

        public PatientRepository(DataContext context)
        {
            _context = context;
        }

        public List<Patient> GetList()
        {
            return _context.patientList
                        .Include(p => p.Treatment) 
                        .Include(c => c.Files) 
                        .ThenInclude(f => f.PatientAttributes) 
                        .ToList();
        }

        public void Add(Patient patient)
        {
            _context.patientList.Add(patient);
            _context.SaveChanges();
        }

     

        public Patient GetById(int id) // ✅ מימוש הפונקציה
        {


            return _context.patientList
              .Include(p => p.Files)
                .FirstOrDefault(p => p.Id == id);

            }

        public Patient GetByIdentity(string identity)
        {
            return _context.patientList
                   .Include(p => p.Files) 
                   .ThenInclude(f => f.PatientAttributes)
                   .FirstOrDefault(p => p.PatientIdentity == identity);

        }
        public void DeletePatient(int id)
        {
            var patient = _context.patientList.Find(id);
            _context.Remove(patient);
            _context.SaveChanges();
        }

        public void DeletePatientByNumberID(string identity)
        {
            var patient = _context.patientList.FirstOrDefault(p=>p.PatientIdentity==identity);

            _context.Remove(patient);
            _context.SaveChanges();
        }


        public void UpdatePatient(Patient patient)
        {
            _context.patientList.Update(patient);
            _context.SaveChanges();
        }


    }


}

