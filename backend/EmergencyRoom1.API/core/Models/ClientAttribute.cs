using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace core.Models
{
    public class ClientAttribute
    {
       

        [Key] 
        public int Id { get; set; }

        public string AttributeName { get; set; }
        public double Score { get; set; }

        [DefaultValue(false)]

        [JsonIgnore]
        public ICollection<PatientFile> PatientFiles{ get; set; } = new List<PatientFile>();
    }
       
}
