using BusinessLogicLayer.Models.Response;
using BusinessLogicLayer.Services.Interfaces;
using System;
using System.Net.NetworkInformation;

namespace BusinessLogicLayer.Services
{
    public class CheckPingService : ICheckPingService
    {
        private static double nAllSend;
        private static double nTrueSend;
        private static double nFalseSend;
        private static double percents;
        public PingResponseModel getPercent()
        {
            PingResponseModel pingResponseModel = new PingResponseModel();

            getPing();

            if (DateTime.Now.Hour == 16 && DateTime.Now.Minute == 55 && DateTime.Now.Second >= 0 && DateTime.Now.Second <= 2)
            {

                if (nAllSend != 0)
                {
                    percents = nTrueSend * 100 / nAllSend;
                    percents = Math.Round(percents, 2);
                }

            }

            if (DateTime.Now.Hour == 16 && DateTime.Now.Minute == 55 && DateTime.Now.Second >= 3 && DateTime.Now.Second <= 5)
            {
                nAllSend = 0;
                nTrueSend = 0;
                nFalseSend = 0;
            }

            pingResponseModel.percents = percents;

            return pingResponseModel;
        }

        private void getPing()
        {
            try
            {
                Ping ping = new Ping();

                PingReply reply = ping.Send("a0005-s");

                bool pingable = reply.Status == IPStatus.Success;

                if (pingable)
                {
                    nTrueSend++;
                }

                if (!pingable)
                {
                    nFalseSend++;
                }

                nAllSend++;
            }

            catch
            {

            }
        }
    }
}