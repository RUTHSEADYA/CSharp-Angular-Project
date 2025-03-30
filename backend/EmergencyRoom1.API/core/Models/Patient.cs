    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Text.Json.Serialization;
    using System.Threading.Tasks;

    namespace core.Models
    {
        public class Patient
        {

            [Key] // מפתח ראשי
            public int Id { get; set; }
            
           public string ?PatientIdentity { get; set; }  

            [Required]
            public string? Name { get; set; }

            public string? Gender { get; set; }

        public DateTime DateOfBirth { get; set; }

            public string? Address { get; set; }

            public string ?Phone { get; set; }
            public string? Email { get; set; }

           public ICollection<PatientFile>? Files { get; set; } = new List<PatientFile>();
            public DateTime EntryTime { get; set; }
        public Treatment? Treatment { get; set; }

        // קשר של אחד-לרבים (מטופל אחד -> הרבה טיפולים)
        //public List<Treatment> Treatments { get; set; } = new List<Treatment>();



    }
    }
