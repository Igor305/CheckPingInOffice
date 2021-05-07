using BusinessLogicLayer.Models.Response;

namespace BusinessLogicLayer.Services.Interfaces
{
    public interface ICheckPingService
    {
        public PingResponseModel getPercent();
    }
}
