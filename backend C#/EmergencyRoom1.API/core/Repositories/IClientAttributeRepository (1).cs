using core.Models;


namespace core.Repositories
{
    public interface IClientAttributeRepository
    {

        List<ClientAttribute> GetList();
        void Add(ClientAttribute clientAttribute);

        ClientAttribute GetById(int id); 

        void DeleteClientAttribute(int id);

        ClientAttribute GetAttributeByName(string attributeName);





    }
}
