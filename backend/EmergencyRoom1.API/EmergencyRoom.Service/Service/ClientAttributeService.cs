using EmergencyRoom.Core.Models;
using EmergencyRoom.Core.Repositories;
using EmergencyRoom.Core.Service;
using EmergencyRoom.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmergencyRoom.Service.Service
{
    public class ClientAttributeService : IClientAttributeService
    {

       
        private readonly IClientAttributeRepository _clientAttributeReposirory;

        public ClientAttributeService(IClientAttributeRepository clientAttributeReposirory)
        {
            _clientAttributeReposirory = clientAttributeReposirory;
        }

        public List<ClientAttribute> GetAll() {
            return _clientAttributeReposirory.GetList();
        }



    }

   
}
