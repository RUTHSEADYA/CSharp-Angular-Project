using EmergencyRoom.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmergencyRoom.Data
{
    public class DataContext
    {


     public List<ClientAttribute> clientAttribute { get; set; }

        public DataContext()
        {
            clientAttribute = new List<ClientAttribute>();
        }

  
    }

}
