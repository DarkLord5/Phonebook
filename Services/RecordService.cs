using Microsoft.EntityFrameworkCore;
using Phonebook.Data;
using Phonebook.Models;
using Phonebook.ViewModels;

namespace Phonebook.Services
{
    public class RecordService : IRecordService
    {
        private readonly PhonebookContext _context;

        public RecordService(PhonebookContext context)
        {
            _context = context;
        }

        private async Task<List<Subdivision>> GetSubdivisionsAsync(Record record) //Subdivision hierarchy for each record
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

        private async Task<List<RecordViewModel>> ConvertToViewModel(List<Record> records) //Converter
        {
            var recordViewModelList = new List<RecordViewModel>();

            foreach (var record in records)
            {

                var recordViewModel = new RecordViewModel()
                {
                    Id = record.Id,
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



        private async Task<List<RecordViewModel>> GetAllRecordsAsync() //return all records
        {
            var records = await _context.Records.ToListAsync();

            return await ConvertToViewModel(records);
        }


        public async Task<List<RecordViewModel>> SearchAndGetRecordsAsync(string name,
                string surname, string fathername, string position, string phonenumber) //Search by parametrs
        {
            var records = await _context.Records.ToListAsync();

            if (!string.IsNullOrEmpty(name))
            {
                records = records.Where(r => r.Name.ToLower() == name.ToLower()).ToList();
            }

            if (!string.IsNullOrEmpty(surname))
            {
                records = records.Where(r => r.Surname.ToLower() == surname.ToLower()).ToList();
            }

            if (!string.IsNullOrEmpty(fathername))
            {
                records = records.Where(r => r.FatherName.ToLower() == fathername.ToLower()).ToList();
            }

            if (!string.IsNullOrEmpty(position))
            {
                records = records.Where(r => r.Position.ToLower() == position.ToLower()).ToList();
            }

            if (!string.IsNullOrEmpty(phonenumber))
            {
                records = records.Where(r => r.PersonalNumber.Contains(phonenumber) ||
                    r.WorkNumber.Contains(phonenumber) || r.WorkMobileNumber.Contains(phonenumber)).ToList();
            }

            return await ConvertToViewModel(records);
        }

        private async Task<List<Record>> GetSubdivis(int id) //Рекурсивный метод для получения иерархии
        {
            var records = await _context.Records.Where(r => r.SubdivisionID == id).ToListAsync();

            var subDivs = await _context.Subdivisions.Where(s => s.ParentId == id).ToListAsync();

            foreach (var subdiv in subDivs)
            {
                records.AddRange(await GetSubdivis(subdiv.Id));
            }

            return records;
        }

        public async Task<List<RecordViewModel>?> FindBySubdivisionAsync(int subId)
        {
            var subDiv = await _context.Subdivisions.FindAsync(subId);

            if (subDiv != null)
            {
                var records = await GetSubdivis(subDiv.Id);

                return await ConvertToViewModel(records);
            }

            return null;
            throw new Exception("There is no such subdivision");
        }



        public async Task<List<RecordViewModel>?> CreateRecordAsync(Record record)
        {
            if (string.IsNullOrEmpty(record.Name) || string.IsNullOrEmpty(record.Surname) || string.IsNullOrEmpty(record.FatherName)
                || string.IsNullOrEmpty(record.Position) || (!await _context.Subdivisions.AnyAsync(s => s.Id == record.SubdivisionID))
                || (record.PersonalNumber == null) || (record.WorkNumber == null) || (record.WorkMobileNumber == null))
            {
                return null;
            }

            _context.Records.Add(record);

            await _context.SaveChangesAsync();

            return await GetAllRecordsAsync();
        }


        public async Task<List<RecordViewModel>?> UpdateRecordAsync(Record record, int id)
        {

            var check = await _context.Records.AnyAsync(r => r.Id == id);

            if (check && !string.IsNullOrEmpty(record.Name) && !string.IsNullOrEmpty(record.Surname)
                && !string.IsNullOrEmpty(record.FatherName) && !string.IsNullOrEmpty(record.Position)
                && (await _context.Subdivisions.AnyAsync(s => s.Id == record.SubdivisionID)) && (record.PersonalNumber != null)
                && (record.WorkNumber != null) && (record.WorkMobileNumber != null))
            {
                var oldRecord = await _context.Records.Where(r => r.Id == id).FirstAsync();

                oldRecord.Name = record.Name;
                oldRecord.Surname = record.Surname;
                oldRecord.FatherName = record.FatherName;
                oldRecord.Position = record.Position;
                oldRecord.SubdivisionID = record.SubdivisionID;
                oldRecord.PersonalNumber = record.PersonalNumber;
                oldRecord.WorkNumber = record.WorkNumber;
                oldRecord.WorkMobileNumber = record.WorkMobileNumber;

                _context.Entry(oldRecord).State = EntityState.Modified;

                await _context.SaveChangesAsync();

                return await GetAllRecordsAsync();
            }

            return null;
        }


        public async Task<List<RecordViewModel>?> DeleteRecordsAsync(List<int> idList)
        {
            if (idList.Count == 0)
            {
                return null;
            }

            foreach (var id in idList)
            {
                if (await _context.Records.Where(r => r.Id == id).AnyAsync())
                {
                    var recordToDelete = await _context.Records.Where(r => r.Id == id).FirstAsync();

                    _context.Records.Remove(recordToDelete);
                }
            }

            await _context.SaveChangesAsync();

            return await GetAllRecordsAsync();
        }
    }
}