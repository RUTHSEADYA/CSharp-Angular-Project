using core.Models;
using core.Repositories;


namespace data.Repositories
{
    public class ClientAttributeRepository: IClientAttributeRepository
    {

        private readonly DataContext _context;

        public ClientAttributeRepository(DataContext context)
        {
            _context = context;
        }

        public List<ClientAttribute> GetList()
        {
            return _context.clientAttributeList.ToList(); ;

        }

        public void Add(ClientAttribute clientAttribute)
        {
            _context.clientAttributeList.Add(clientAttribute);
            _context.SaveChanges();
        }

        public ClientAttribute GetById(int id) // ✅ מימוש הפונקציה לשליפת טופס לפי מזהה
        {
            return _context.clientAttributeList.FirstOrDefault(p => p.Id == id);
        }

        public void DeleteClientAttribute(int id)
        {
            ClientAttribute c=_context.clientAttributeList.Find(id);
            _context.Remove(c);
            _context.SaveChanges();
             }

        public ClientAttribute GetAttributeByName(string attributeName)
        {
            return _context.clientAttributeList.FirstOrDefault(attr => attr.AttributeName == attributeName);
        }


    }
}
