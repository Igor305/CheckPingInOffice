using System.Net;
using System.Net.NetworkInformation;

namespace PingForPC
{
    public class PingModel
    {
        public long RoundtripTime { get; set; }
        public string Status { get; set; }
        public int? Ttl { get; set; }
        public IPAddress Address { get; set; }
    }
}
