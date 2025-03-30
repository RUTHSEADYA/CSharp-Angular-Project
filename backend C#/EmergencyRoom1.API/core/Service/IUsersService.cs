using core.Models;


namespace core.Service
{
    public interface IUsersService
    {
        List<Users> GetAll();
        void AddUser(Users user);
        Users GetUserByUsername(string username); 


    }
}
