using Phonebook.Models;
using Phonebook.ViewModels;

namespace Phonebook.Services
{
    public interface IRecordService
    {
        public Task<List<RecordViewModel>> SearchAndGetRecordsAsync(string name, 
                string surname, string fathername, string position, string phonenumber);
        public Task<List<RecordViewModel>?> CreateRecordAsync(Record record);
        public Task<List<RecordViewModel>?> UpdateRecordAsync(Record record, int id);
        public Task<List<RecordViewModel>?> DeleteRecordsAsync(List<int> idList);
        public Task<List<RecordViewModel>?> FindBySubdivisionAsync(int subId);
    }
}
