using EmergencyRoom.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmergencyRoom.Data.Repositories
{
    public class ClientAttributeRepository
    {

        private readonly DataContext _context;

        public ClientAttributeRepository(DataContext context)
        {
            _context = context;
        }

        public List<ClientAttribute> GetList()
        {
            return _context.clientAttribute.ToList(); ;

        }

      
    }
}
