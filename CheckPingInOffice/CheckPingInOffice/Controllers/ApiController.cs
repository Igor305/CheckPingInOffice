using BusinessLogicLayer.Models.Response;
using BusinessLogicLayer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CheckPingInOffice.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly ICheckApiService _chechApiService;

        public ApiController(ICheckApiService chechApiService)
        {
            _chechApiService = chechApiService;
        }

        [HttpGet]
        public PingResponseModel getPersentApi([FromQuery] string name, [FromQuery] string path)
        {
            PingResponseModel pingResponseModel = _chechApiService.getPersent(name,path);

            return pingResponseModel;
        }

        [HttpGet("getApi")]
        public ApiResponseModel getApi()
        {
            ApiResponseModel apiResponseModel = _chechApiService.getApi();

            return apiResponseModel;
        }

        [HttpGet("addApi")]
        public IpResponseModel addApi([FromQuery] string name, [FromQuery] string path)
        {
            IpResponseModel ipResponseModel = _chechApiService.addApi(name, path);

            return ipResponseModel;
        }
        [HttpGet("updateApi")]
        public IpResponseModel updateApi([FromQuery] string path, [FromQuery] string pathNew)
        {
            IpResponseModel ipResponseModel = _chechApiService.updateApi(path, pathNew);

            return ipResponseModel;
        }

        [HttpGet("deleteApi")]
        public IpResponseModel deleteApi([FromQuery] string name, [FromQuery] string path)
        {
            IpResponseModel ipResponseModel = _chechApiService.deleteApi(name, path);

            return ipResponseModel;
        }
    }
}
