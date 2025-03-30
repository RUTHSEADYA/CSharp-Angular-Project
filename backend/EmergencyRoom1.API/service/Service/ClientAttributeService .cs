using core.Models;
using core.Repositories;
using core.Service;


namespace service.Service
{
    public class ClientAttributeService : IClientAttributeService

    {

       
        private readonly IClientAttributeRepository _clientAttributeReposirory;
        private readonly IPatientRepository _patientReposirory;


        public ClientAttributeService(IClientAttributeRepository clientAttributeReposirory, IPatientRepository patientReposirory)
        {
            _clientAttributeReposirory = clientAttributeReposirory;
            _patientReposirory= patientReposirory;
        }

        public List<ClientAttribute> GetAll() {
            return _clientAttributeReposirory.GetList();
        }

        public void AddClientAttribute(ClientAttribute clientAttribute)
        {
            _clientAttributeReposirory.Add(clientAttribute);
        }
        
             

        public ClientAttribute GetById(int id) 
        {
             return _clientAttributeReposirory.GetById(id);
        }

        public void DeleteClientAttribute(int id)
        {
             _clientAttributeReposirory.DeleteClientAttribute(id);
        }

















    }

   
}
