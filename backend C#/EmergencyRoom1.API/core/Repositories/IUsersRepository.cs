using core.Models;


namespace core.Repositories
{
    public interface IUsersRepository
    {
        List<Users> GetList();
        void Add(Users user);
        Users GetUserByUsername(string username); 


    }
}
