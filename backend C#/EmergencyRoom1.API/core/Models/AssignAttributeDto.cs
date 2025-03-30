using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.Models
{
    public class AssignAttributeDto
    {
        public int PatientId { get; set; }
        public int ClientAttributeId { get; set; }
        public bool Selected { get; set; }

    }
}
