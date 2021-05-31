using BusinessLogicLayer.Models.Response;

namespace BusinessLogicLayer.Services.Interfaces
{
   public interface ICheckApiService
    {
        public PingResponseModel getPersent(string name, string path);
        public ApiResponseModel getApi();
        public IpResponseModel addApi(string name, string path);
        public IpResponseModel updateApi(string path, string pathNew);
        public IpResponseModel deleteApi(string name, string path);
    }
}
