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
        public PingResponseModel Ping(string ip)
        {
            PingResponseModel pingResponseModel = _checkPingService.getPercent(ip);

            return pingResponseModel;
        }

        [HttpGet("getIp")]
        public IpAddressResponseModel getIp()
        {
            IpAddressResponseModel ipAddressResponseModel = _checkPingService.getIp();

            return ipAddressResponseModel;
        }

        [HttpGet("addIp")]
        public IpResponseModel addIp(string ip)
        {
            IpResponseModel ipResponseModel = _checkPingService.addIp(ip);

            return ipResponseModel;
        }

        [HttpGet("updateIp")]
        public IpResponseModel updateIp([FromQuery] string ip,[FromQuery] string ipNew)
        {
            IpResponseModel ipResponseModel =_checkPingService.updateIp(ip, ipNew);

            return ipResponseModel;
        }

        [HttpGet("deleteIp")]
        public IpResponseModel deleteIp([FromQuery] string ip)
        {
            IpResponseModel ipResponseModel = _checkPingService.deleteIp(ip);

            return ipResponseModel;
        }
    }
}
