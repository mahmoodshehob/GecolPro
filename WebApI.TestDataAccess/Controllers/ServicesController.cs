using ClassLibrary.DataAccess.Interfaces;
using ClassLibrary.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApI.TestDataAccess.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServicesController : ControllerBase
    {

        private readonly IMeterService _meterService;
        private readonly IRequestService _requestService;

        public ServicesController(IMeterService meterService, IRequestService requestService)
        {
            _meterService = meterService;
            _requestService = requestService;
        }

        [HttpGet("IsMeterExist/{meterNumber}")]
        public async Task<IActionResult> IsMeterExist(string meterNumber)
        {
            var result = await _meterService.IsExist(meterNumber);
            return Ok(result);
        }


        [HttpGet("CreateNewMeter/{meterNumber}/{at}/{tt}")]
        public async Task<IActionResult> CreateNew(string meterNumber, string at, string tt)
        {
            var result = await _meterService.CreateNew(meterNumber,at,tt);
            return Ok(result);
        }


        [HttpGet("GetAllMeters")]
        public async Task<IActionResult> GetAllMeters()
        {
            var result = await _meterService.GetAll();
            return Ok(result);
        }


        [HttpGet("SaveDcblRequest/{conversationId}/{MSISDN}/{amount}/{status}/{transactionId}")]
        public async Task<IActionResult> SaveDcblRequest(string? conversationId, string? MSISDN, string amount, bool status, string transactionId)
        {
            var result = await _requestService.SaveDcblRequest(  conversationId,   MSISDN,  amount, status,  transactionId);
            return Ok(result);
        }


        [HttpGet("SaveGecolRequest/{conversationId}/{MSISDN}/{amount}/{status}/{token}/{uniqueNumber}")]
        public async Task<IActionResult> SaveGecolRequest(string? conversationId, string? MSISDN, string amount,
            bool status, string token, string uniqueNumber)
        {
            var result = await _requestService.SaveGecolRequest(conversationId, MSISDN, amount, status, token,uniqueNumber);
            return Ok(result);
        }

        [HttpGet("GetAllRequests")]
        public async Task<IActionResult> GetAllRequests()
        {
            var result = await _requestService.GetAll();
            return Ok(result);
        }



    }
}