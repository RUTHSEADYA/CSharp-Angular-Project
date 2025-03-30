using core.Models;
using core.Repositories;
using Microsoft.EntityFrameworkCore;


namespace data.Repositories
{
    public class UsersRepository : IUsersRepository
    {

        private readonly DataContext _context;

        public UsersRepository(DataContext context)
        {
            _context = context;
        }

        public List<Users> GetList()
        {
            return _context.usersList.ToList(); ;

        }

        public void Add(Users user)
        {
            try
            {
                _context.usersList.Add(user);
                try
                {
                    _context.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine($"❌ DbUpdateException: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"➡️ Inner Exception: {ex.InnerException.Message}");
                    }
                    throw; 
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error saving user: {ex.Message}");
                throw; 
            }

        }
        public Users GetUserByUsername(string username)
        {
            return _context.usersList.FirstOrDefault(u => u.Name == username);
        }
    }
}

