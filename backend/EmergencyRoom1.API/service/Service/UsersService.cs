using core.Models;
using core.Repositories;
using core.Service;


namespace service.Service
{
    public class UsersService: IUsersService
    {


        private readonly IUsersRepository _usersReposirory;

        public UsersService(IUsersRepository usersReposirory)
        {
            _usersReposirory = usersReposirory;
        }

        public List<Users> GetAll()
        {
            return _usersReposirory.GetList();
        }

        public void AddUser(Users user)
        {
            _usersReposirory.Add(user);
        }

        public Users GetUserByUsername(string username)
        {
            return _usersReposirory.GetUserByUsername(username);
        }
    }
}
