using Phonebook.Models;

namespace Phonebook.ViewModels
{
    public class RecordViewModel
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? FatherName { get; set; }
        public string? Position { get; set; }
        public List<string>? WorkNumber { get; set; }
        public List<string>? PersonalNumber { get; set; }
        public List<string>? WorkMobileNumber { get; set; }
        public List<Subdivision>? Subdivision { get; set; }
    }
}
