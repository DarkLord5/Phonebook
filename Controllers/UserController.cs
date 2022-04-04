using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Phonebook.Filters;
using Phonebook.Models;
using Phonebook.Services;
using Phonebook.ViewModels;

namespace Phonebook.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        readonly private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost]
        [Route("api/Registration")]
        [PhonebookAsyncExceptionFilter]
        public async Task<ActionResult<User>> Registration(RegistrationViewModel newUser)
        {
            var result = await _userService.Registration(newUser);

            if(result == null)
            {
                throw new Exception("Registration failed!");
            }

            return result;
        }

        [HttpPost]
        [Route("api/Login")]
        [PhonebookAsyncExceptionFilter]
        public async Task<ActionResult<bool>> Login(LoginViewModel user)
        {
            var result = await _userService.Login(user);
            if (result)
            {
                return result;
            }

            throw new Exception("Authorization failed!");
        }

        [HttpPost]
        [Route("api/Logout")]
        [Authorize(Roles = "admin,user")]
        public async Task Logout() => await _userService.Logout();
    }
}
