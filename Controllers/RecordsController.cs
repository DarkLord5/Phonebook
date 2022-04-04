using Microsoft.AspNetCore.Mvc;
using Phonebook.Filters;
using Phonebook.Models;
using Phonebook.Services;
using Phonebook.ViewModels;

namespace Phonebook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecordsController : ControllerBase
    {
        private readonly IRecordService _recordService;

        public RecordsController(IRecordService recordService)
        {
            _recordService = recordService;
        }

        // GET: api/Records?.....
        [HttpGet]
        public async Task<ActionResult<List<RecordViewModel>>> GetFilteredRecords(string? name,
                string? surname, string? fathername, string? position, string? phonenumber)
        {
            return await _recordService.SearchAndGetRecordsAsync(name, surname, fathername, position, phonenumber);
        }


        [PhonebookAsyncExceptionFilter]
        [HttpGet("{id}")]
        public async Task<ActionResult<List<RecordViewModel>>> GetSubdivisionRecords(int id)
        {
            var result = await _recordService.FindBySubdivisionAsync(id);

            if (result == null)
            {
                throw new Exception("Ivalid value to update!");
            }

            return result;
        }


        [PhonebookAsyncExceptionFilter]
        [HttpPut("{id}")]
        public async Task<ActionResult<List<RecordViewModel>>> UpdateRecord(int id, Record record)
        {
            var result = await _recordService.UpdateRecordAsync(record, id);

            if (result == null)
            {
                throw new Exception("Ivalid value to update!");
            }

            return result;
        }


        [PhonebookAsyncExceptionFilter]
        [HttpPost]
        public async Task<ActionResult<List<RecordViewModel>>> CreateRecord(Record record)
        {
            var result = await _recordService.CreateRecordAsync(record);

            if (result == null)
            {
                throw new Exception("Invalid entry value - one or more required fields are empty!");
            }

            return result;
        }

        // DELETE: api/Records/5
        [PhonebookAsyncExceptionFilter]
        [HttpDelete]
        public async Task<ActionResult<List<RecordViewModel>?>> DeleteRecord(List<int> idList)
        {
            var result = await _recordService.DeleteRecordsAsync(idList);

            if (result == null)
            {
                throw new Exception("This list is empty!");
            }

            return result;
        }
    }
}
