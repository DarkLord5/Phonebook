using Microsoft.EntityFrameworkCore;
using Phonebook.Data;
using Phonebook.Models;

namespace Phonebook.Services
{
    public class SubdivisionService : ISubdivisionService
    {
        private readonly PhonebookContext _phonebookContext;

        public SubdivisionService(PhonebookContext phonebookContext)
        {
            _phonebookContext = phonebookContext;
        }
        public async Task<List<Subdivision>> GetAllSubdivisionsAsync() => 
            await _phonebookContext.Subdivisions.ToListAsync();

        public async Task<Subdivision> CreateSubdivisionAsync(Subdivision subdivision)
        {

            if (!string.IsNullOrEmpty(subdivision.Name))
            {
                _phonebookContext.Subdivisions.Add(subdivision);

                await _phonebookContext.SaveChangesAsync();

                return subdivision;
            }

            throw new Exception("This subdivision is empty!");
        }

        public async Task<List<Subdivision>> DeleteSubdivisionAsync(int id)
        {
            var subdiv = await _phonebookContext.Subdivisions.FindAsync(id);

            if (subdiv == null)
            {
                throw new Exception("There is no such subdivision!");
            }

            _phonebookContext.Subdivisions.Remove(subdiv);

            await _phonebookContext.SaveChangesAsync();

            return await GetAllSubdivisionsAsync();
        }

        

        public async Task<Subdivision> UpdateSubdivisionAsync(int id, Subdivision subdivision)
        {
            var newSubdiv = new Subdivision() { Id = id, Name = subdivision.Name };

            if (string.IsNullOrEmpty(newSubdiv.Name))
            {
                throw new Exception("Your new subdivision is empty!");
            }

            if (!_phonebookContext.Subdivisions.Any(e => e.Id == id))
                throw new Exception("There is no such subdivision!");

            _phonebookContext.Entry(newSubdiv).State = EntityState.Modified;

            await _phonebookContext.SaveChangesAsync();

            return newSubdiv;
        }
    }
}
