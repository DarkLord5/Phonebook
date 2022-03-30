using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<User>> Registration(RegistrationViewModel newUser) => Ok(await _userService.Registration(newUser));

        [HttpPost]
        [Route("api/Login")]
        public async Task<ActionResult<bool>> Login(LoginViewModel user) => Ok(await _userService.Login(user));

        [HttpPost]
        [Route("api/Logout")]
        [Authorize(Roles = "admin,user")]
        public async Task Logout() => await _userService.Logout();
    }
}
