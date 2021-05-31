using BusinessLogicLayer.Models;
using BusinessLogicLayer.Models.Response;
using BusinessLogicLayer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Text;
using System.Timers;

namespace BusinessLogicLayer.Services
{
    public class CheckPingService : ICheckPingService
    {
        private static bool isStart;
        private static bool isRead;
        private static readonly string path = "YesterdayIp.txt";

        private static List<AllVariablesModel> allVariablesModels = new List<AllVariablesModel>();

        public PingResponseModel getPercent(string name, string ip)
        {
            PingResponseModel pingResponseModel = new PingResponseModel();

            try
            {
                AllVariablesModel connect = allVariablesModels.Find(x => x.ipAddress == ip);

                if (connect != null)
                {

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
            }
            catch
            {

            }

            return pingResponseModel;
        }

        public IpAddressResponseModel getIp()
        {
            IpAddressResponseModel  ipAddressResponseModels = new IpAddressResponseModel();

            try
            {

                start();

                foreach (AllVariablesModel ipAddress in allVariablesModels)
                {
                    ipAddressResponseModels.ipAddress.Add(new IpAddressModel { nameConnect = ipAddress.nameConnect, ipAddress = ipAddress.ipAddress });
                    
                    if(ipAddress.nAllSendYesterday == 0)
                    {
                        isRead = true;
                    }
                }

                if (isRead)
                {
                    readInFile();
                    isRead = false;
                }
            }

            catch
            {

            }

            return ipAddressResponseModels;
        }

        private void start()
        {
            if (!isStart)
            {
                isStart = true;

                allVariablesModels.Add(new AllVariablesModel
                {
                    nameConnect = "Office",
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
        }

        private void readInFile()
        {
            try
            {
                if (File.Exists(path))
                {
                    var text = File.ReadAllLines(path, Encoding.UTF8);

                    AllVariablesModel connect = new AllVariablesModel();

                    foreach (string str in text)
                    {
                        if (str.Contains("ipAddress:"))
                        {
                            string ip = str.Substring(10);

                            connect = allVariablesModels.Find(x => x.ipAddress == ip);

                        }

                        if (connect != null)
                        {
                            if (str.Contains("nAllSendYesterday:"))
                            {
                                double nAllSendYesterday = Double.Parse(str.Substring(18));

                                connect.nAllSendYesterday = nAllSendYesterday;
                            }

                            if (str.Contains("nFalseSendYesterday:"))
                            {
                                double nFalseSendYesterday = Double.Parse(str.Substring(20));

                                connect.nFalseSendYesterday = nFalseSendYesterday;
                            }

                            if (str.Contains("nTrueSendYesterday:"))
                            {
                                double nTrueSendYesterday = Double.Parse(str.Substring(19));

                                connect.nTrueSendYesterday = nTrueSendYesterday;
                            }

                            if (str.Contains("percentsYesterday:"))
                            {
                                double percentsYesterday = Double.Parse(str.Substring(18));

                                connect.percentsYesterday = percentsYesterday;
                            }
                        }
                    }
                }
            }

            catch
            {

            }
        }

        public IpResponseModel addIp(string name, string ip)
        {
            IpResponseModel ipResponseModel = new IpResponseModel();

            if (name == null)
            {
                ipResponseModel.response = $"Вы забыли ввести имя адреса!";
                return ipResponseModel;
            }

            if (ip == null)
            {
                ipResponseModel.response = $"Вы забыли ввести ip!";
                return ipResponseModel;
            }

            AllVariablesModel isIp = allVariablesModels.Find(x => x.ipAddress == ip);


            if (isIp != null)
            {
                ipResponseModel.response = $"Ip:{ip} уже существует!";

                return ipResponseModel;
            }

            allVariablesModels.Add(new AllVariablesModel
            {
                nameConnect = name,
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

            ipResponseModel.response = $"Ip-адрес: {name} - {ip} успешно добавлен!";
            
            return ipResponseModel;
        }

        public IpResponseModel updateIp(string ip, string ipNew)
        {
            IpResponseModel ipResponseModel = new IpResponseModel();

            AllVariablesModel connectcopy = allVariablesModels.Find(x => x.ipAddress == ipNew);

            if (connectcopy != null)
            {
                ipResponseModel.response = $"Данный IP:{ipNew} уже пингуется!";

                return ipResponseModel;
            }

            if (ipNew == null)
            {
                ipResponseModel.response = $"Введите новый IP!";

                return ipResponseModel;
            }

            if (ip == ipNew)
            {
                ipResponseModel.response = $"Cтарый и новый IP совпадают:{ip}, измените IP!";
                
                return ipResponseModel;
            }

            AllVariablesModel connect = allVariablesModels.Find(x => x.ipAddress == ip);

            if (connect == null)
            {
                ipResponseModel.response = $"Выберете IP!";

                return ipResponseModel;
            }

            connect.timer.Stop();
            connect.timer.Dispose();

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

            connect.timer = new Timer(1000);
            connect.timer.Elapsed += (o, e) => getPing(ipNew);
            connect.timer.AutoReset = true;
            connect.timer.Start();

            ipResponseModel.response = $"новый ipAddress: {ipNew} удачно установлен!";


            return ipResponseModel;
        }

        public IpResponseModel deleteIp(string name, string ip)
        {
            IpResponseModel ipResponseModel = new IpResponseModel();

            AllVariablesModel connect = allVariablesModels.Find(x => x.ipAddress == ip);

            if (connect == null)
            {
                ipResponseModel.response = $"Ip: {ip} не был найден!";

                return ipResponseModel;
            }

            connect.timer.Stop();

            allVariablesModels.Remove(connect);

            ipResponseModel.response = $"Ip-адрес:{name} - {ip} был успешно удалён!";
            
            return ipResponseModel;
        }

        private void getPing(string ip)
        {
            try
            {
                AllVariablesModel connect = allVariablesModels.Find(x => x.ipAddress == ip);

                if (DateTime.Now.Hour == 8 && DateTime.Now.Minute == 0 && DateTime.Now.Second == 0)
                {
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }

                if (DateTime.Now.Hour == 8 && DateTime.Now.Minute == 0 && DateTime.Now.Second == 1)
                {

                    if (connect.nAllSendForDay != 0)
                    {
                        connect.percentsYesterday = connect.nTrueSendForDay * 100 / connect.nAllSendForDay;
                        connect.percentsYesterday = Math.Round(connect.percentsYesterday, 2);

                        connect.nAllSendYesterday = connect.nAllSendForDay;
                        connect.nTrueSendYesterday = connect.nTrueSendForDay;
                        connect.nFalseSendYesterday = connect.nFalseSendForDay;

                    }
     
                    string text = $"****************************\nipAddress:{connect.ipAddress}\nnAllSendYesterday:{connect.nAllSendYesterday}\nnFalseSendYesterday:{connect.nFalseSendYesterday}\nnTrueSendYesterday:{connect.nTrueSendYesterday}\npercentsYesterday:{connect.percentsYesterday}\n";

                    File.AppendAllText(path, text);
                  
                    connect.nAllSendForDay = 0;
                    connect.nTrueSendForDay = 0;
                    connect.nFalseSendForDay = 0;
                }

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