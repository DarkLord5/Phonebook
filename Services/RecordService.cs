using Microsoft.EntityFrameworkCore;
using Phonebook.Data;
using Phonebook.Models;
using Phonebook.ViewModels;
using System.Linq;

namespace Phonebook.Services
{
    public class RecordService : IRecordService
    {
        private readonly PhonebookContext _context;

        public RecordService(PhonebookContext context)
        {
            _context = context;
        }

        private async Task<List<Subdivision>> GetSubdivisionsAsync(Record record)
        {
            var currentSub = await _context.Subdivisions.Where(s => s.Id == record.SubdivisionID).FirstAsync();

            var subList = new List<Subdivision>() { currentSub };

            while (currentSub.ParentId != null)
            {
                currentSub = await _context.Subdivisions.Where(s => s.Id == currentSub.ParentId).FirstAsync();

                subList.Add(currentSub);
            }

            return subList;
        }

        private async Task<List<RecordViewModel>> ConvertToViewModel(List<Record> records)
        {
            var recordViewModelList = new List<RecordViewModel>();

            foreach (var record in records)
            {

                var recordViewModel = new RecordViewModel()
                {
                    Name = record.Name,
                    Surname = record.Surname,
                    FatherName = record.FatherName,
                    PersonalNumber = record.PersonalNumber,
                    Position = record.Position,
                    WorkNumber = record.WorkNumber,
                    WorkMobileNumber = record.WorkMobileNumber,
                    Subdivision = await GetSubdivisionsAsync(record)
                };

                recordViewModelList.Add(recordViewModel);
            }

            return recordViewModelList;
        }


        public async Task<List<RecordViewModel>> GetAllRecordsAsync()
        {
            var records = await _context.Records.ToListAsync();

            return await ConvertToViewModel(records);
        }


        public async Task<List<RecordViewModel>> CreateRecordAsync(Record record)
        {
            if (record == null)//добавить доп проверки
            {
                throw new ArgumentNullException(nameof(record)); //Поменять.
            }

            _context.Records.Add(record);

            await _context.SaveChangesAsync();

            return await GetAllRecordsAsync();
        }

        public async Task<List<RecordViewModel>> UpdateRecordAsync(Record record, int id)
        {
            var oldRecord = await _context.Records.Where(r => r.Id == id).FirstAsync();

            if (record.Id == oldRecord.Id) // добавить доп проверки
            {
                _context.Entry(record).State = EntityState.Modified;

                await _context.SaveChangesAsync();
            }

            return await GetAllRecordsAsync();
        }


        public async Task<List<RecordViewModel>> DeleteRecordsAsync(List<int> idList)
        {
            foreach (int id in idList)
            {
                var recordToDelete = await _context.Records.Where(r => r.Id == id).FirstAsync();

                if (recordToDelete != null)
                {
                    _context.Records.Remove(recordToDelete);
                }
            }

            await _context.SaveChangesAsync();

            return await GetAllRecordsAsync();
        }



        public async Task<List<RecordViewModel>> SearchAndGetRecordsAsync(Record parameters)
        {
            var records = await _context.Records.ToListAsync();

            if (!string.IsNullOrEmpty(parameters.Name))
            {
                records = records.Where(r => r.Name.ToLower() == parameters.Name.ToLower()).ToList();
            }

            if (!string.IsNullOrEmpty(parameters.Surname))
            {
                records = records.Where(r => r.Surname.ToLower() == parameters.Surname.ToLower()).ToList();
            }

            if (!string.IsNullOrEmpty(parameters.FatherName))
            {
                records = records.Where(r => r.FatherName.ToLower() == parameters.FatherName.ToLower()).ToList();
            }

            if (!string.IsNullOrEmpty(parameters.Position))
            {
                records = records.Where(r => r.Position.ToLower() == parameters.Position.ToLower()).ToList();
            }

            if (parameters.Subdivision != null)
                if (!string.IsNullOrEmpty(parameters.Subdivision.Name))
                {
                    var subdivList = await _context.Subdivisions.Where(s => s.Name.ToLower() == parameters.Subdivision.Name.ToLower()).
                        Select(s => s.Id).ToListAsync();

                    if (subdivList != null)
                        records = records.Where(r => subdivList.Contains(r.SubdivisionID)).ToList();
                }

            if (parameters.PersonalNumber != null)
            {
                string phone = parameters.PersonalNumber.First();

                records = records.Where(r => r.PersonalNumber.Contains(phone) ||
                    r.WorkNumber.Contains(phone) || r.WorkMobileNumber.Contains(phone)).ToList();
            }


            return await ConvertToViewModel(records);
        }


    }
}
