namespace Phonebook.Models
{
    public class Record
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? FatherName { get; set; }
        public string? Position { get; set; }
        public List<string>? WorkNumber { get; set; }
        public List<string>? PersonalNumber { get; set; }
        public List<string>? WorkMobileNumber { get; set; }
        public int? SubdivisionID { get; set; }
        public Subdivision? Subdivision { get; set; }
    }
}
