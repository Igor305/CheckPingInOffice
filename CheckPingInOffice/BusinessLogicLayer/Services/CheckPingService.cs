using BusinessLogicLayer.Models.Response;
using BusinessLogicLayer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Timers;

namespace BusinessLogicLayer.Services
{
    public class CheckPingService : ICheckPingService
    {
        private static bool isStart;
        private static Timer aTimer;

        private static string ipAddress = "10.15.8.1";

        private static double nAllSendLastHour = 0;
        private static double nTrueSendLastHour = 0;
        private static double nFalseSendLastHour = 0;
        private static double percentsLastHour = 0;
        private static List<bool> lastHour = new List<bool>();

        private static double nAllSendForDay = 0;
        private static double nTrueSendForDay = 0;
        private static double nFalseSendForDay = 0;
        private static double percentsForDay = 0; 

        private static double nAllSendYesterday = 0;
        private static double nTrueSendYesterday = 0;
        private static double nFalseSendYesterday = 0;
        private static double percentsYesterday = 0;

        public PingResponseModel getPercent()
        {
            PingResponseModel pingResponseModel = new PingResponseModel();

            if (!isStart)
            {
                isStart = true;

                aTimer = new Timer(1000);
                aTimer.Elapsed += getPing;
                aTimer.AutoReset = true;
                aTimer.Start();
            }

            if (DateTime.Now.Hour == 8 && DateTime.Now.Minute == 0 && DateTime.Now.Second >= 0 && DateTime.Now.Second <= 1)
            {

                if (nAllSendForDay != 0)
                {
                    percentsYesterday = nTrueSendForDay * 100 / nAllSendForDay;
                    percentsYesterday = Math.Round(percentsYesterday, 2);

                    nAllSendYesterday = nAllSendForDay;
                    nTrueSendYesterday = nTrueSendForDay;
                    nFalseSendYesterday = nFalseSendForDay;
                }

            }

            if (DateTime.Now.Hour == 8 && DateTime.Now.Minute == 0 && DateTime.Now.Second >= 2 && DateTime.Now.Second <= 3)
            {
                nAllSendForDay = 0;
                nTrueSendForDay = 0;
                nFalseSendForDay = 0;
            }

            if (nAllSendForDay != 0)
            {
                percentsForDay = nTrueSendForDay * 100 / nAllSendForDay;
                percentsForDay = Math.Round(percentsForDay, 2);
            }

            if (nAllSendLastHour != 0)
            {
                percentsLastHour = nTrueSendLastHour * 100 / nAllSendLastHour;
                percentsLastHour = Math.Round(percentsLastHour, 2);
            }

            pingResponseModel.nAllSendLastHour = nAllSendLastHour;
            pingResponseModel.nTrueSendLastHour = nTrueSendLastHour;
            pingResponseModel.nFalseSendLastHour = nFalseSendLastHour;
            pingResponseModel.percentsLastHour = percentsLastHour;

            pingResponseModel.nAllSendForDay = nAllSendForDay;
            pingResponseModel.nFalseSendForDay = nFalseSendForDay;
            pingResponseModel.nTrueSendForDay = nTrueSendForDay;
            pingResponseModel.percentsForDay = percentsForDay;

            pingResponseModel.nAllSendYesterday = nAllSendYesterday;
            pingResponseModel.nFalseSendYesterday = nFalseSendYesterday;
            pingResponseModel.nTrueSendYesterday = nTrueSendYesterday;
            pingResponseModel.percentsYesterday = percentsYesterday;

            return pingResponseModel;
        }

        public string getIp()
        {
            return ipAddress;
        }

        public void setIp(string ip)
        {
            ipAddress = ip;
        }

        private void getPing(Object source, ElapsedEventArgs e)
        {
            try
            {
                Ping ping = new Ping();

                PingReply reply = ping.Send(ipAddress);

                bool pingable = reply.Status == IPStatus.Success;

                if (lastHour.Count >= 3600)
                {
                    if (lastHour[0])
                    {
                        nTrueSendLastHour--;
                    }

                    if (!lastHour[0])
                    {
                        nFalseSendLastHour--;
                    }

                    lastHour.RemoveAt(0);
                }

                if (pingable)
                {
                    lastHour.Add(true);

                    nTrueSendLastHour++;
                    nTrueSendForDay++;
                }

                if (!pingable)
                {
                    lastHour.Add(false);

                    nFalseSendLastHour++;
                    nFalseSendForDay++;
                }

                nAllSendLastHour = lastHour.Count;

                nAllSendForDay++;
            }

            catch
            {

            }
        }
    }
}