using BusinessLogicLayer.Models.Response;

namespace BusinessLogicLayer.Services.Interfaces
{
    public interface ICheckPingService
    {
        public PingResponseModel getPercent(string name, string ip);
        public IpAddressResponseModel getIp();
        public IpResponseModel addIp(string name, string ip);
        public IpResponseModel updateIp(string ip, string ipNew);
        public IpResponseModel deleteIp(string name, string ip);
    }
}
