using BusinessLogicLayer.Models;
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

        private static List<AllVariablesModel> allVariablesModels = new List<AllVariablesModel>();

        public PingResponseModel getPercent(string ip)
        {
            PingResponseModel pingResponseModel = new PingResponseModel();

            AllVariablesModel connect = allVariablesModels.Find(x => x.ipAddress == ip);

            if (connect != null)
            {

                if (DateTime.Now.Hour == 8 && DateTime.Now.Minute == 0 && DateTime.Now.Second >= 0 && DateTime.Now.Second <= 1)
                {

                    if (connect.nAllSendForDay != 0)
                    {
                        connect.percentsYesterday = connect.nTrueSendForDay * 100 / connect.nAllSendForDay;
                        connect.percentsYesterday = Math.Round(connect.percentsYesterday, 2);

                        connect.nAllSendYesterday = connect.nAllSendForDay;
                        connect.nTrueSendYesterday = connect.nTrueSendForDay;
                        connect.nFalseSendYesterday = connect.nFalseSendForDay;
                    }

                }

                if (DateTime.Now.Hour == 8 && DateTime.Now.Minute == 0 && DateTime.Now.Second >= 2 && DateTime.Now.Second <= 3)
                {
                    connect.nAllSendForDay = 0;
                    connect.nTrueSendForDay = 0;
                    connect.nFalseSendForDay = 0;
                }

                if (connect.nAllSendForDay != 0)
                {
                    connect.percentsForDay = connect.nTrueSendForDay * 100 / connect.nAllSendForDay;
                    connect.percentsForDay = Math.Round(connect.percentsForDay, 2);
                }

                if (connect.nAllSendLastHour != 0)
                {
                    connect.percentsLastHour = connect.nTrueSendLastHour * 100 / connect.nAllSendLastHour;
                    connect.percentsLastHour = Math.Round(connect.percentsLastHour, 2);
                }


                pingResponseModel.nAllSendLastHour = connect.nAllSendLastHour;
                pingResponseModel.nTrueSendLastHour = connect.nTrueSendLastHour;
                pingResponseModel.nFalseSendLastHour = connect.nFalseSendLastHour;
                pingResponseModel.percentsLastHour = connect.percentsLastHour;

                pingResponseModel.nAllSendForDay = connect.nAllSendForDay;
                pingResponseModel.nFalseSendForDay = connect.nFalseSendForDay;
                pingResponseModel.nTrueSendForDay = connect.nTrueSendForDay;
                pingResponseModel.percentsForDay = connect.percentsForDay;

                pingResponseModel.nAllSendYesterday = connect.nAllSendYesterday;
                pingResponseModel.nFalseSendYesterday = connect.nFalseSendYesterday;
                pingResponseModel.nTrueSendYesterday = connect.nTrueSendYesterday;
                pingResponseModel.percentsYesterday = connect.percentsYesterday;
            }

            return pingResponseModel;
        }

        public IpAddressResponseModel getIp()
        {
            IpAddressResponseModel  ipAddressResponseModels = new IpAddressResponseModel();

            if (!isStart)
            {
                isStart = true;

                allVariablesModels.Add(new AllVariablesModel
                {
                    ipAddress = "10.15.8.1",

                    nAllSendLastHour = 0,
                    nTrueSendLastHour = 0,
                    nFalseSendLastHour = 0,
                    percentsLastHour = 0,

                    nAllSendForDay = 0,
                    nTrueSendForDay = 0,
                    nFalseSendForDay = 0,
                    percentsForDay = 0,

                    nAllSendYesterday = 0,
                    nTrueSendYesterday = 0,
                    nFalseSendYesterday = 0,
                    percentsYesterday = 0,

                    timer = new Timer(1000)
                });

                AllVariablesModel allVariablesModel = allVariablesModels.Find(x => x.ipAddress == "10.15.8.1");

                allVariablesModel.timer.Elapsed += (o, e) => getPing("10.15.8.1");
                allVariablesModel.timer.AutoReset = true;
                allVariablesModel.timer.Start();
            }

            foreach (AllVariablesModel ipAddress in allVariablesModels)
            {
                ipAddressResponseModels.ipAddress.Add(ipAddress.ipAddress);
            }

            return ipAddressResponseModels;
        }

        public IpResponseModel addIp(string ip)
        {
            IpResponseModel ipResponseModel = new IpResponseModel();

            if (ip != null)
            {         

                AllVariablesModel isIp = allVariablesModels.Find(x => x.ipAddress == ip);

                if (isIp == null)
                {
                    allVariablesModels.Add(new AllVariablesModel
                    {
                        ipAddress = ip,

                        nAllSendLastHour = 0,
                        nTrueSendLastHour = 0,
                        nFalseSendLastHour = 0,
                        percentsLastHour = 0,

                        nAllSendForDay = 0,
                        nTrueSendForDay = 0,
                        nFalseSendForDay = 0,
                        percentsForDay = 0,

                        nAllSendYesterday = 0,
                        nTrueSendYesterday = 0,
                        nFalseSendYesterday = 0,
                        percentsYesterday = 0,

                        timer = new Timer(1000)
                    });

                    AllVariablesModel allVariablesModel = allVariablesModels.Find(x => x.ipAddress == ip);

                    allVariablesModel.timer.Elapsed += (o, e) => getPing(ip);
                    allVariablesModel.timer.AutoReset = true;
                    allVariablesModel.timer.Start();

                    ipResponseModel.response = $"IP: {ip} успешно добавлен!";
                }

                if (isIp != null)
                {
                    ipResponseModel.response = $"IP: {ip} уже существует!";
                }
            }
            else
            {
                ipResponseModel.response = $"Вы забыли ввести ipAddress!";
            }

            return ipResponseModel;
        }

        public IpResponseModel updateIp(string ip, string ipNew)
        {
            IpResponseModel ipResponseModel = new IpResponseModel();

            AllVariablesModel connect = allVariablesModels.Find(x => x.ipAddress == ip);

            connect.timer.Stop();

            System.Threading.Thread.Sleep(5000);

            connect.ipAddress = ipNew;

            connect.nAllSendLastHour = 0;
            connect.nTrueSendLastHour = 0;
            connect.nFalseSendLastHour = 0;
            connect.percentsLastHour = 0;
            connect.lastHour.Clear();

            connect.nAllSendForDay = 0;
            connect.nTrueSendForDay = 0;
            connect.nFalseSendForDay = 0;
            connect.percentsForDay = 0;

            connect.nAllSendYesterday = 0;
            connect.nTrueSendYesterday = 0;
            connect.nFalseSendYesterday = 0;
            connect.percentsYesterday = 0;

            connect.timer.Start();

            ipResponseModel.response = $"новый ipAddress: {ipNew} удачно установлен";

            return ipResponseModel;
        }

        public IpResponseModel deleteIp(string ip)
        {
            IpResponseModel ipResponseModel = new IpResponseModel();

            AllVariablesModel connect = allVariablesModels.Find(x => x.ipAddress == ip);

            if (connect == null)
            {
                ipResponseModel.response = $"Ip: {ip} не был найден!";
            }

            if (connect != null)
            {
                connect.timer.Stop();

                System.Threading.Thread.Sleep(2000);

                allVariablesModels.Remove(connect);

                ipResponseModel.response = $"Ip: {ip} был успешно удалён!";
            }
            
            return ipResponseModel;
        }

        private void getPing(string ip)
        {
            try
            {
                AllVariablesModel connect = allVariablesModels.Find(x => x.ipAddress == ip);

                Ping ping = new Ping();

                PingReply reply = ping.Send(connect.ipAddress);

                bool pingable = reply.Status == IPStatus.Success;

                if (connect.lastHour.Count >= 3600)
                {
                    if (connect.lastHour[0])
                    {
                        connect.nTrueSendLastHour--;
                    }

                    if (!connect.lastHour[0])
                    {
                        connect.nFalseSendLastHour--;
                    }

                    connect.lastHour.RemoveAt(0);
                }

                if (pingable)
                {
                    connect.lastHour.Add(true);

                    connect.nTrueSendLastHour++;
                    connect.nTrueSendForDay++;
                }

                if (!pingable)
                {
                    connect.lastHour.Add(false);

                    connect.nFalseSendLastHour++;
                    connect.nFalseSendForDay++;
                }

                connect.nAllSendLastHour = connect.lastHour.Count;

                connect.nAllSendForDay++;
            }

            catch
            {

            }
        }
    }
}