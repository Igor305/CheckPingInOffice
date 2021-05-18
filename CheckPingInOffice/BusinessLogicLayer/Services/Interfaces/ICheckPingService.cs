using BusinessLogicLayer.Models.Response;

namespace BusinessLogicLayer.Services.Interfaces
{
    public interface ICheckPingService
    {
        public PingResponseModel getPercent();
        public string getIp();
        public void setIp(string ip);
    }
}
