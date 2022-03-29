namespace Phonebook.Models
{
    public class Subdivision
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? ParentId { get; set; }
        public Subdivision? Parent { get; set; }
    }
}
