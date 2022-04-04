using Phonebook.Models;

namespace Phonebook.Services
{
    public interface ISubdivisionService
    {
        public Task<List<Subdivision>> GetAllSubdivisionsAsync();
        public Task<Subdivision?> CreateSubdivisionAsync(Subdivision subdivision);
        public Task<Subdivision?> UpdateSubdivisionAsync(int id, Subdivision subdivision);
        public Task<List<Subdivision>?> DeleteSubdivisionAsync(int id);
    }
}
