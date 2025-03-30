using core.Models;
using core.Service;
using Microsoft.AspNetCore.Mvc;


namespace apiProject.Controllers
{
    [Route("api/Users/")]
    [ApiController]
    public class UsersController : ControllerBase
    {


        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpGet]
        public IEnumerable<Users> Get()
        {
            return _usersService.GetAll();
        }


   
    }
}

