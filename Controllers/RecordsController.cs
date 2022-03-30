using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("{id}")]
        public async Task<ActionResult<List<RecordViewModel>>> GetSubdivisionRecords(int id)
        {
            return await _recordService.FindBySubdivisionAsync(id);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<List<RecordViewModel>>> UpdateRecord(int id, Record record)
        {
            return await _recordService.UpdateRecordAsync(record, id);
        }



        [HttpPost]
        public async Task<ActionResult<List<RecordViewModel>>> CreateRecord(Record record)
        {
            return await _recordService.CreateRecordAsync(record);
        }

        // DELETE: api/Records/5
        [HttpDelete]
        public async Task<ActionResult<List<RecordViewModel>>> DeleteRecord(List<int> idList)
        {
            return await _recordService.DeleteRecordsAsync(idList);
        }
    }
}
