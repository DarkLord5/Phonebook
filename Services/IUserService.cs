using Phonebook.Models;
using Phonebook.ViewModels;

namespace Phonebook.Services
{
    public interface IUserService
    {
        public Task<User> Registration(RegistrationViewModel newUser);
        public Task<bool> Login(LoginViewModel user);
        public Task Logout();
    }
}