using core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace data
{
    public class DataContext:DbContext
    {
        public DataContext()
        {
        }

        public DataContext(DbContextOptions<DataContext> options) : base(options){}
        public DbSet<ClientAttribute> clientAttributeList { get; set; }

        public DbSet<Patient> patientList { get; set; }

        public DbSet<Users> usersList { get; set; }

        public DbSet<PatientFile> patientFileList { get; set; }

        public DbSet<Treatment> Treatments { get; set; }



    }

}
