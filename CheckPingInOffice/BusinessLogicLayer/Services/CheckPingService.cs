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

        private static Timer timerSaveFile = new Timer();

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
                    
                    if(ipAddress.nAllSendLastHour == 0)
                    {
                        isRead = true;
                    }
                }

                if (isRead)
                {
                    isRead = false;
                    readInFile();
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

                timerSaveFile = new Timer(30000);
                timerSaveFile.Elapsed += (o, e) => writeInFile();
                timerSaveFile.AutoReset = true;
                timerSaveFile.Start();
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

                    string ip = "";
                    string name = "";
                    double nAllSendYesterday = 0;
                    double nFalseSendYesterday = 0;
                    double nTrueSendYesterday = 0;
                    double percentsYesterday = 0;
                    double nAllSendForDay = 0;
                    double nFalseSendForDay = 0;
                    double nTrueSendForDay = 0;
                    double percentsForDay = 0;
                    double nAllSendLastHour = 0;
                    double nFalseSendLastHour = 0;
                    double nTrueSendLastHour = 0;
                    double percentsLastHour = 0;

                    foreach (string str in text)
                    {
                        if (str.Contains("ipAddress:"))
                        {
                            ip = str.Substring(10);

                            connect = allVariablesModels.Find(x => x.ipAddress == ip);
                        }

                        if (connect != null)
                        {
                            if (str.Contains("name:"))
                            {
                                name = str.Substring(5);

                                connect.nameConnect = name;
                            }

                            if (str.Contains("nAllSendYesterday:"))
                            {
                                nAllSendYesterday = Double.Parse(str.Substring(18));

                                connect.nAllSendYesterday = nAllSendYesterday;
                            }

                            if (str.Contains("nFalseSendYesterday:"))
                            {
                                nFalseSendYesterday = Double.Parse(str.Substring(20));

                                connect.nFalseSendYesterday = nFalseSendYesterday;
                            }

                            if (str.Contains("nTrueSendYesterday:"))
                            {
                                nTrueSendYesterday = Double.Parse(str.Substring(19));

                                connect.nTrueSendYesterday = nTrueSendYesterday;
                            }

                            if (str.Contains("percentsYesterday:"))
                            {
                                percentsYesterday = Double.Parse(str.Substring(18));

                                connect.percentsYesterday = percentsYesterday;
                            }
                            if (str.Contains("nAllSendForDay:"))
                            {
                                nAllSendForDay = Double.Parse(str.Substring(15));

                                connect.nAllSendForDay = nAllSendForDay;
                            }

                            if (str.Contains("nFalseSendForDay:"))
                            {
                                nFalseSendForDay = Double.Parse(str.Substring(17));

                                connect.nFalseSendForDay = nFalseSendForDay;
                            }

                            if (str.Contains("nTrueSendForDay:"))
                            {
                                nTrueSendForDay = Double.Parse(str.Substring(16));

                                connect.nTrueSendForDay = nTrueSendForDay;
                            }

                            if (str.Contains("percentsForDay:"))
                            {
                                percentsForDay = Double.Parse(str.Substring(15));

                                connect.percentsForDay = percentsForDay;
                            }

                            if (str.Contains("nAllSendLastHour:"))
                            {
                                nAllSendLastHour = Double.Parse(str.Substring(17));

                                connect.nAllSendLastHour = nAllSendLastHour;
                            }

                            if (str.Contains("nFalseSendLastHour:"))
                            {
                                nFalseSendLastHour = Double.Parse(str.Substring(19));

                                connect.nFalseSendLastHour = nFalseSendLastHour;
                            }

                            if (str.Contains("nTrueSendLastHour:"))
                            {
                                nTrueSendLastHour = Double.Parse(str.Substring(18));

                                connect.nTrueSendLastHour = nTrueSendLastHour;
                            }

                            if (str.Contains("percentsLastHour:"))
                            {
                                percentsLastHour = Double.Parse(str.Substring(17));

                                connect.percentsLastHour = percentsLastHour;

                                List<bool> lasthour = new List<bool>();

                                for (int x = 0; x < connect.nTrueSendLastHour; x++)
                                {
                                    lasthour.Add(true);
                                }

                                for (int x = 0; x < connect.nFalseSendLastHour; x++)
                                {
                                    lasthour.Add(false);
                                }

                                connect.lastHour = lasthour;
                            }
                        }

                        if (connect == null)
                        {

                            if (str.Contains("name:"))
                            {
                                name = str.Substring(5);
                            }

                            if (str.Contains("nAllSendYesterday:"))
                            {
                                nAllSendYesterday = Double.Parse(str.Substring(18));
                            }

                            if (str.Contains("nFalseSendYesterday:"))
                            {
                                nFalseSendYesterday = Double.Parse(str.Substring(20));
                            }

                            if (str.Contains("nTrueSendYesterday:"))
                            {
                                nTrueSendYesterday = Double.Parse(str.Substring(19));
                            }

                            if (str.Contains("percentsYesterday:"))
                            {
                                percentsYesterday = Double.Parse(str.Substring(18));
                            }
                            if (str.Contains("nAllSendForDay:"))
                            {
                                nAllSendForDay = Double.Parse(str.Substring(15));
                            }

                            if (str.Contains("nFalseSendForDay:"))
                            {
                                nFalseSendForDay = Double.Parse(str.Substring(17));
                            }

                            if (str.Contains("nTrueSendForDay:"))
                            {
                                nTrueSendForDay = Double.Parse(str.Substring(16));
                            }

                            if (str.Contains("percentsForDay:"))
                            {
                                percentsForDay = Double.Parse(str.Substring(15));
                            }

                            if (str.Contains("nAllSendLastHour:"))
                            {
                                nAllSendLastHour = Double.Parse(str.Substring(17));
                            }

                            if (str.Contains("nFalseSendLastHour:"))
                            {
                                nFalseSendLastHour = Double.Parse(str.Substring(19));
                            }

                            if (str.Contains("nTrueSendLastHour:"))
                            {
                                nTrueSendLastHour = Double.Parse(str.Substring(18));
                            }

                            List<bool> lasthour = new List<bool>();

                            if (str.Contains("percentsLastHour:"))
                            {
                                percentsLastHour = Double.Parse(str.Substring(17));

                                for (int x = 0; x < nTrueSendLastHour; x++)
                                {
                                    lasthour.Add(true);
                                }

                                for (int x = 0; x < nFalseSendLastHour; x++)
                                {
                                    lasthour.Add(false);
                                }

                                allVariablesModels.Add(new AllVariablesModel
                                {
                                    nameConnect = name,
                                    ipAddress = ip,

                                    nAllSendLastHour = nAllSendLastHour,
                                    nTrueSendLastHour = nTrueSendLastHour,
                                    nFalseSendLastHour = nFalseSendLastHour,
                                    percentsLastHour = percentsLastHour,
                                    lastHour = lasthour,

                                    nAllSendForDay = nAllSendForDay,
                                    nTrueSendForDay = nTrueSendForDay,
                                    nFalseSendForDay = nFalseSendForDay,
                                    percentsForDay = percentsForDay,

                                    nAllSendYesterday = nAllSendYesterday,
                                    nTrueSendYesterday = nTrueSendYesterday,
                                    nFalseSendYesterday = nFalseSendYesterday,
                                    percentsYesterday = percentsYesterday,

                                    timer = new Timer(1000)
                                });

                                AllVariablesModel allVariablesModel = allVariablesModels.Find(x => x.ipAddress == ip);

                                allVariablesModel.timer.Elapsed += (o, e) => getPing(allVariablesModel.ipAddress);
                                allVariablesModel.timer.AutoReset = true;
                                allVariablesModel.timer.Start();
                            }                         
                        }
                    }
                }
            }

            catch
            {

            }
        }

        

        private void writeInFile()
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                string text = "";

                foreach (AllVariablesModel connect in allVariablesModels)
                {
                    text = $"****************************\nipAddress:{connect.ipAddress}\nname:{connect.nameConnect}\n" +
                    $"nAllSendYesterday:{connect.nAllSendYesterday}\nnFalseSendYesterday:{connect.nFalseSendYesterday}\nnTrueSendYesterday:{connect.nTrueSendYesterday}\npercentsYesterday:{connect.percentsYesterday}\n" +
                    $"nAllSendForDay:{connect.nAllSendForDay}\nnFalseSendForDay:{connect.nFalseSendForDay}\nnTrueSendForDay:{connect.nTrueSendForDay}\npercentsForDay:{connect.percentsForDay}\n" +
                    $"nAllSendLastHour:{connect.nAllSendLastHour}\nnFalseSendLastHour:{connect.nFalseSendLastHour}\nnTrueSendLastHour:{connect.nTrueSendLastHour}\npercentsLastHour:{connect.percentsLastHour}\n";

                    File.AppendAllText(path, text);
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

                    if (connect.nAllSendForDay != 0)
                    {
                        connect.percentsYesterday = connect.nTrueSendForDay * 100 / connect.nAllSendForDay;
                        connect.percentsYesterday = Math.Round(connect.percentsYesterday, 2);

                        connect.nAllSendYesterday = connect.nAllSendForDay;
                        connect.nTrueSendYesterday = connect.nTrueSendForDay;
                        connect.nFalseSendYesterday = connect.nFalseSendForDay;

                    }
                  
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