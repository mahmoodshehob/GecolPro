using Microsoft.AspNetCore.Mvc;
using GecolPro.BusinessRules.Interfaces;
using GecolPro.DataAccess.Interfaces;
using GecolPro.Models.DbEntity;

namespace GecolPro.WebApi.Controllers
{
    [ApiController]
    [Route("api/[Controller]/")]
    public class CheckController : ControllerBase
    {
        private readonly IUssdProcess _ussdProcess;
        private readonly IDbUnitOfWork _dbUnitOfWork;


        public CheckController(IUssdProcess ussdProcess , IDbUnitOfWork dbUnitOfWork) 
        {
            _ussdProcess = ussdProcess;
            _dbUnitOfWork = dbUnitOfWork;
        }


        private async Task<string> RemoteIpAddress()
        {
            string? remoteIpAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            if (remoteIpAddress == "127.0.0.1" || remoteIpAddress == "::1")
            {
                // Get the original IP from the X-Forwarded-For header
                remoteIpAddress = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            }
            return remoteIpAddress;
        }



        #region API Region 


        [HttpGet("VendService", Name = "VendService")]

        public async Task<IActionResult> VendService()
        {

            bool Status = await _ussdProcess.CheckServiceExist(await RemoteIpAddress());

            string responseTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

            if (Status)
            {
                return Ok(new
                {
                    ResponseTime = responseTime,
                    Status = "Service Connected"
                });
            }
            else
            {
                return BadRequest(new
                {
                    ResponseTime = responseTime,
                    Status = "Service Down"
                });
            }
        }
        
        //

        [HttpGet()]
        [Route("[Action]")]
        public async Task<IActionResult> CheckMeter(string meter)
        {

           var MeterDet = await _ussdProcess.CheckMeterInGecolWithDet(meter);

            string responseTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

            if (MeterDet.Item1)
            {
                return Ok(new
                {
                    ResponseTime = responseTime,
                    Status = "Success",
                    Result = MeterDet.Item2
                });
            }
            else
            {
                return BadRequest(new
                {
                    ResponseTime = responseTime,
                    Status = "Failed",
                    Result = MeterDet.Item2
                });
            }
        }
        //

        [HttpGet("DcbService",Name = "DcbService")]
        public async Task<IActionResult> DcbService()
        {
            bool Status = await _ussdProcess.CheckDcbExist();

            string responseTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

            if (Status)
            {
                return Ok(new
                {
                    ResponseTime = responseTime,
                    Status = "Service Connected"
                });
            }
            else
            {
                return BadRequest(new
                {
                    ResponseTime = responseTime,
                    Status = "Service Down"
                });
            }
        }

        //

        [HttpGet("DatabaseService", Name = "DatabaseService")]
        public async Task<IActionResult> DatabaseService()
        {
            var serviceResult = await _dbUnitOfWork.DatabaseConnection.CheckConnection();

            string responseTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

            if (serviceResult.Status)
            {
                return Ok(new {
                    ResponseTime = responseTime,
                    Status = "Service Connected" });
            }
            else
            {
                return BadRequest(new
                {
                    ResponseTime = responseTime,
                    Status = "Service Down",
                    Reason = serviceResult.Message
                });
            }
        }


        #endregion

    }
}
