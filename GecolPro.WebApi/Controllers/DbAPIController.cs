using GecolPro.DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GecolPro.WebApi.Controllers
{
    public class DbAPIController : ControllerBase
    {

        private readonly IDbUnitOfWork? _unitOfWork;

        public DbAPIController(IDbUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("IsMeterExist/{meterNumber}")]
        public async Task<IActionResult> IsMeterExist(string meterNumber)
        {
            var result = await _unitOfWork.Meter.IsExist(meterNumber);

            if (result.Status)
                return Ok(result);
            
            return BadRequest(result);
            
        }


        [HttpGet("CreateNewMeter/{meterNumber}/{at}/{tt}")]
        public async Task<IActionResult> CreateNew(string meterNumber, string at, string tt)
        {
            var result = await _unitOfWork.Meter.CreateNew(meterNumber, at, tt);

            if (result.Status)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }


        [HttpGet("GetAllMeters")]
        public async Task<IActionResult> GetAllMeters()
        {
            var result = await _unitOfWork.Meter.GetAll();
            return Ok(result);
        }


        [HttpGet("SaveDcblRequest/{conversationId}/{MSISDN}/{amount}/{status}/{transactionId}")]
        public async Task<IActionResult> SaveDcblRequest(string? conversationId, string? MSISDN, string meterNumber, string amount, bool status, string transactionId)
        {
            var result = await _unitOfWork.Request.SaveDcblRequest(conversationId, MSISDN, meterNumber, amount, status, transactionId);
            return Ok(result);
        }


        [HttpGet("SaveGecolRequest/{conversationId}/{MSISDN}/{amount}/{status}/{token}/{uniqueNumber}/{totalTax}")]
        public async Task<IActionResult> SaveGecolRequest(string? conversationId, string? MSISDN, string? meterNumber, string amount,
            bool status,string transactionId, [FromQuery] string[] token,
             string uniqueNumber, string totalTax)
        {
            var result = await _unitOfWork.Request.SaveGecolRequest(conversationId, MSISDN, meterNumber, amount, status, transactionId, token, uniqueNumber, totalTax);
            return Ok(result);
        }

        [HttpGet("GetAllRequests")]
        public async Task<IActionResult> GetAllRequests()
        {
            var result = await _unitOfWork.Request.GetAll();
            return Ok(result);
        }


        [HttpGet("QueryTokenHistory")]
        public async Task<IActionResult> QueryTokenHistoryRequests(string Msisdn)
        {
            var result = await _unitOfWork.Request.QueryTokenHistoryAll(Msisdn,30);
            return Ok(result);
        }




        [HttpGet("CreateNewIssueToken/{conversationId}/{msisdn}/{dateTimeReq}/{uniqueNumber}/{meterNumber}/{amount}")]
        public async Task<IActionResult> CreateNewIssueToken(string? conversationId, string? msisdn, string? dateTimeReq, string? uniqueNumber, string? meterNumber, int amount)
        {
            var result = await _unitOfWork.IssueToken.CreateNew(conversationId, msisdn, dateTimeReq, uniqueNumber, meterNumber, amount);
            return Ok(result);
        }


        [HttpGet("GetAllIssueTokens")]
        public async Task<IActionResult> GetAllIssueTokens()
        {
            var result = await _unitOfWork.IssueToken.GetAll();
            return Ok(result);
        }

    }
}