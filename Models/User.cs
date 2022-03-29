using Microsoft.AspNetCore.Identity;

namespace Phonebook.Models
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? SecondName { get; set; }
    }
}