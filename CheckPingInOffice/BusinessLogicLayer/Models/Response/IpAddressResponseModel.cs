using System.Collections.Generic;

namespace BusinessLogicLayer.Models.Response
{
    public class IpAddressResponseModel
    {
        public List<string> ipAddress  { get; set; }

        public IpAddressResponseModel()
        {
            ipAddress = new List<string> ();
        }
    }
}
