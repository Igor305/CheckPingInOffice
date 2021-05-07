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
    }
}
