using System.Net;
using System.Net.Http;

namespace PingForPC
{
    public class PingApiModel
    {
        public int Milliseconds { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccessStatusCode { get; set; }
        public HttpRequestMessage? RequestMessage { get; set; }
    }
}
