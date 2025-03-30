using core.Models;


namespace core.Service
{
    public interface IClientAttributeService
    {
        List<ClientAttribute> GetAll();
        void AddClientAttribute(ClientAttribute clientAttribute);
        ClientAttribute GetById(int id);
        void DeleteClientAttribute(int id);


    }
}
