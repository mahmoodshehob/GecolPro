﻿using GecolPro.DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GecolPro.WebApi.Controllers
{
    partial class DbAPIController : ControllerBase
    {

        private readonly IUnitOfWork? _unitOfWork;

        public DbAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("IsMeterExist/{meterNumber}")]
        public async Task<IActionResult> IsMeterExist(string meterNumber)
        {
            var result = await _unitOfWork.Meter.IsExist(meterNumber);
            return Ok(result);
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
        public async Task<IActionResult> SaveDcblRequest(string? conversationId, string? MSISDN, string amount, bool status, string transactionId)
        {
            var result = await _unitOfWork.Request.SaveDcblRequest(conversationId, MSISDN, amount, status, transactionId);
            return Ok(result);
        }


        [HttpGet("SaveGecolRequest/{conversationId}/{MSISDN}/{amount}/{status}/{token}/{uniqueNumber}/{totalTax}")]
        public async Task<IActionResult> SaveGecolRequest(string? conversationId, string? MSISDN, string amount,
            bool status, [FromQuery] string[] token,
             string uniqueNumber, string totalTax)
        {
            var result = await _unitOfWork.Request.SaveGecolRequest(conversationId, MSISDN, amount, status, token, uniqueNumber, totalTax);
            return Ok(result);
        }

        [HttpGet("GetAllRequests")]
        public async Task<IActionResult> GetAllRequests()
        {
            var result = await _unitOfWork.Request.GetAll();
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