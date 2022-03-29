using Phonebook.Models;
using Phonebook.ViewModels;

namespace Phonebook.Services
{
    public interface IRecordService
    {
        public Task<List<RecordViewModel>> GetAllRecordsAsync();
        public Task<List<RecordViewModel>> SearchAndGetRecordsAsync(Record parameters);
        public Task<List<RecordViewModel>> CreateRecordAsync(Record record);
        public Task<List<RecordViewModel>> UpdateRecordAsync(Record record, int id);
        public Task<List<RecordViewModel>> DeleteRecordsAsync(List<int> idList);
        //public Task<List<RecordViewModel>> SortAndGetRecords();
    }
}
