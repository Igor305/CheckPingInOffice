using BusinessLogicLayer.Models.Response;

namespace BusinessLogicLayer.Services.Interfaces
{
    public interface ICheckPingService
    {
        public PingResponseModel getPercent(string ip);
        public IpAddressResponseModel getIp();
        public IpResponseModel addIp(string ip);
        public IpResponseModel updateIp(string ip, string ipNew);
        public IpResponseModel deleteIp(string ip);
    }
}
