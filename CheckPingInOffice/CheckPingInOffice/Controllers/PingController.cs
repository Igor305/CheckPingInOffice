using BusinessLogicLayer.Models.Response;
using BusinessLogicLayer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CheckPingInOffice.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PingController : ControllerBase
    {
        private readonly ICheckPingService _checkPingService;

        public PingController(ICheckPingService checkPingService)
        {
            _checkPingService = checkPingService;
        }

        [HttpGet]
        public PingResponseModel Ping()
        {
            PingResponseModel pingResponseModel = _checkPingService.getPercent();

            return pingResponseModel;
        }

        [HttpGet("getIp")]
        public IActionResult getIp()
        {
            string ip = _checkPingService.getIp();

            return Ok($"Активный ipAddress = {ip}");
        }

        [HttpGet("setIp")]
        public IActionResult setIp([FromQuery] string ip)
        {
            _checkPingService.setIp(ip);

            return Ok($"Установлен новый ipAddress = {ip}");
        }
    }
}
