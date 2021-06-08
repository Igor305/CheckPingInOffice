using BusinessLogicLayer.Models;
using BusinessLogicLayer.Models.Response;
using BusinessLogicLayer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace BusinessLogicLayer.Services
{
    public class CheckApiService : ICheckApiService
    {
        private static bool isStart;
        private static bool isRead;
        private static readonly string path = "YesterdayApi.txt";
        private static readonly string firstApi = "https://gmsapi.avrora.lan/api/getExcByDocid?api-key=ncbXCefCXParYFM3uwtpX3hYFx5WIdiaINz5B&Docid=1234441";

        private static readonly HttpClient httpClient = new HttpClient();

        private static List<AllVariablesApiModel> allVariablesApiModels = new List<AllVariablesApiModel>();

        public PingResponseModel getPersent(string name, string path)
        {
            PingResponseModel responseModel = new PingResponseModel();
            try
            {
                AllVariablesApiModel allVariablesApiModel = allVariablesApiModels.Find(x => x.path == path);

                if (allVariablesApiModel != null)
                {

                    if (allVariablesApiModel.nAllSendForDay != 0)
                    {
                        allVariablesApiModel.percentsForDay = allVariablesApiModel.nTrueSendForDay * 100 / allVariablesApiModel.nAllSendForDay;
                        allVariablesApiModel.percentsForDay = Math.Round(allVariablesApiModel.percentsForDay, 2);
                    }

                    if (allVariablesApiModel.nAllSendLastHour != 0)
                    {
                        allVariablesApiModel.percentsLastHour = allVariablesApiModel.nTrueSendLastHour * 100 / allVariablesApiModel.nAllSendLastHour;
                        allVariablesApiModel.percentsLastHour = Math.Round(allVariablesApiModel.percentsLastHour, 2);
                    }

                    responseModel.nAllSendLastHour = allVariablesApiModel.nAllSendLastHour;
                    responseModel.nTrueSendLastHour = allVariablesApiModel.nTrueSendLastHour;
                    responseModel.nFalseSendLastHour = allVariablesApiModel.nFalseSendLastHour;
                    responseModel.percentsLastHour = allVariablesApiModel.percentsLastHour;

                    responseModel.nAllSendForDay = allVariablesApiModel.nAllSendForDay;
                    responseModel.nFalseSendForDay = allVariablesApiModel.nFalseSendForDay;
                    responseModel.nTrueSendForDay = allVariablesApiModel.nTrueSendForDay;
                    responseModel.percentsForDay = allVariablesApiModel.percentsForDay;

                    responseModel.nAllSendYesterday = allVariablesApiModel.nAllSendYesterday;
                    responseModel.nFalseSendYesterday = allVariablesApiModel.nFalseSendYesterday;
                    responseModel.nTrueSendYesterday = allVariablesApiModel.nTrueSendYesterday;
                    responseModel.percentsYesterday = allVariablesApiModel.percentsYesterday;
                }
            }
            catch
            {

            }

            return responseModel;
        }
        public ApiResponseModel getApi()
        {
            ApiResponseModel apiModelResponse = new ApiResponseModel();

            try
            {
                start();

                foreach (AllVariablesApiModel allVariablesApiModel in allVariablesApiModels)
                {
                    apiModelResponse.apiModels.Add(new ApiModel
                    {
                        name = allVariablesApiModel.name,
                        path = allVariablesApiModel.path
                    });

                    if (allVariablesApiModel.nAllSendYesterday == 0)
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

            return apiModelResponse;
        }

        private void start()
        {
            if (!isStart)
            {
                isStart = true;

                allVariablesApiModels.Add(new AllVariablesApiModel
                {
                    name = "GMS",
                    path = firstApi,

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


                AllVariablesApiModel allVariablesapiModel = allVariablesApiModels.Find(x => x.path == firstApi);

                allVariablesapiModel.timer.Elapsed += async (o, e) => await getStatusApi(firstApi);
                allVariablesapiModel.timer.AutoReset = true;
                allVariablesapiModel.timer.Start();
            }

        }

        private void readInFile()
        {
            try
            {
                if (File.Exists(path))
                {
                    var text = File.ReadAllLines(path, Encoding.UTF8);

                    AllVariablesApiModel allVariablesApiModel = new AllVariablesApiModel();

                    foreach (string str in text)
                    {
                        if (str.Contains("path:"))
                        {
                            string pathApi = str.Substring(5);

                            allVariablesApiModel = allVariablesApiModels.Find(x => x.path == pathApi);

                        }

                        if (allVariablesApiModel != null)
                        {
                            if (str.Contains("nAllSendYesterday:"))
                            {
                                double nAllSendYesterday = Double.Parse(str.Substring(18));

                                allVariablesApiModel.nAllSendYesterday = nAllSendYesterday;
                            }

                            if (str.Contains("nFalseSendYesterday:"))
                            {
                                double nFalseSendYesterday = Double.Parse(str.Substring(20));

                                allVariablesApiModel.nFalseSendYesterday = nFalseSendYesterday;
                            }

                            if (str.Contains("nTrueSendYesterday:"))
                            {
                                double nTrueSendYesterday = Double.Parse(str.Substring(19));

                                allVariablesApiModel.nTrueSendYesterday = nTrueSendYesterday;
                            }

                            if (str.Contains("percentsYesterday:"))
                            {
                                double percentsYesterday = Double.Parse(str.Substring(18));

                                allVariablesApiModel.percentsYesterday = percentsYesterday;
                            }
                        }
                    }
                }
            }

            catch
            {

            }
        }
   
        public IpResponseModel addApi(string name, string path)
        {
            IpResponseModel ipResponseModel = new IpResponseModel();

            if (name == null)
            {
                ipResponseModel.response = "Вы забыли ввести название";

                return ipResponseModel;
            }

            if (path == null)
            {
                ipResponseModel.response = "Вы забыли ввести путь к api!";

                return ipResponseModel;
            }

            AllVariablesApiModel allVariablesApiModel = allVariablesApiModels.Find(x => x.path == path);

            if (allVariablesApiModel != null)
            {
                ipResponseModel.response = $"Api:{path} уже существует!";

                return ipResponseModel;
            }

            allVariablesApiModels.Add(new AllVariablesApiModel
            {
                name = name,
                path = path,

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

            allVariablesApiModel = allVariablesApiModels.Find(x => x.path == path);

            allVariablesApiModel.timer.Elapsed += async (o, e) => await getStatusApi(path);
            allVariablesApiModel.timer.AutoReset = true;
            allVariablesApiModel.timer.Start();

            ipResponseModel.response = $"Api: {name} - {path} успешно добавлен!";

            return ipResponseModel;
        }

        public IpResponseModel updateApi(string path, string pathNew)
        {
            IpResponseModel ipResponseModel = new IpResponseModel();

            AllVariablesApiModel allVariablesApiModel = allVariablesApiModels.Find(x => x.path == pathNew);

            if (allVariablesApiModel != null)
            {
                ipResponseModel.response = $"Api:{pathNew} уже проверяется!";

                return ipResponseModel;
            }

            if (pathNew == null)
            {
                ipResponseModel.response = $"Введите новый путь!";

                return ipResponseModel;
            }

            if (path == pathNew)
            {
                ipResponseModel.response = $"Cтарый и новый пути совпадают:{path}, измените путь!";

                return ipResponseModel;
            }

            allVariablesApiModel = allVariablesApiModels.Find(x => x.path == path);

            if (allVariablesApiModel == null)
            {
                ipResponseModel.response = $"Выберете Api!";

                return ipResponseModel;
            }

            allVariablesApiModel.timer.Stop();
            allVariablesApiModel.timer.Dispose();

            allVariablesApiModel.path = pathNew;

            allVariablesApiModel.nAllSendLastHour = 0;
            allVariablesApiModel.nTrueSendLastHour = 0;
            allVariablesApiModel.nFalseSendLastHour = 0;
            allVariablesApiModel.percentsLastHour = 0;
            allVariablesApiModel.lastHour.Clear();

            allVariablesApiModel.nAllSendForDay = 0;
            allVariablesApiModel.nTrueSendForDay = 0;
            allVariablesApiModel.nFalseSendForDay = 0;
            allVariablesApiModel.percentsForDay = 0;

            allVariablesApiModel.nAllSendYesterday = 0;
            allVariablesApiModel.nTrueSendYesterday = 0;
            allVariablesApiModel.nFalseSendYesterday = 0;
            allVariablesApiModel.percentsYesterday = 0;

            allVariablesApiModel.timer = new Timer(1000);
            allVariablesApiModel.timer.Elapsed += async (o, e) => await getStatusApi(pathNew);
            allVariablesApiModel.timer.AutoReset = true;
            allVariablesApiModel.timer.Start();

            ipResponseModel.response = $"новый Api: {pathNew} удачно установлен!";


            return ipResponseModel;
        
        }

        public IpResponseModel deleteApi(string name, string path)
        {
            IpResponseModel ipResponseModel = new IpResponseModel();

            AllVariablesApiModel allVariablesApiModel = allVariablesApiModels.Find(x => x.path == path);

            if (allVariablesApiModel == null)
            {
                ipResponseModel.response = $"Api: {path} не был найден!";

                return ipResponseModel;
            }

            allVariablesApiModel.timer.Stop();

            allVariablesApiModels.Remove(allVariablesApiModel);

            ipResponseModel.response = $"Api:{name} - {path} был успешно удалён!";

            return ipResponseModel;
        }

        private async Task getStatusApi(string pathApi)
        {
            try
            {
                AllVariablesApiModel allVariablesApiModel = allVariablesApiModels.Find(x => x.path == pathApi);

                if (DateTime.Now.Hour == 7 && DateTime.Now.Minute == 59)
                {
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }

                if (DateTime.Now.Hour == 8 && DateTime.Now.Minute == 0)
                {

                    if (allVariablesApiModel.nAllSendForDay != 0)
                    {
                        allVariablesApiModel.percentsYesterday = allVariablesApiModel.nTrueSendForDay * 100 / allVariablesApiModel.nAllSendForDay;
                        allVariablesApiModel.percentsYesterday = Math.Round(allVariablesApiModel.percentsYesterday, 2);

                        allVariablesApiModel.nAllSendYesterday = allVariablesApiModel.nAllSendForDay;
                        allVariablesApiModel.nTrueSendYesterday = allVariablesApiModel.nTrueSendForDay;
                        allVariablesApiModel.nFalseSendYesterday = allVariablesApiModel.nFalseSendForDay;

                    }

                    string text = $"****************************\npath:{allVariablesApiModel.path}\nnAllSendYesterday:{allVariablesApiModel.nAllSendYesterday}\nnFalseSendYesterday:{allVariablesApiModel.nFalseSendYesterday}\nnTrueSendYesterday:{allVariablesApiModel.nTrueSendYesterday}\npercentsYesterday:{allVariablesApiModel.percentsYesterday}\n";

                    File.AppendAllText(path, text);

                    allVariablesApiModel.nAllSendForDay = 0;
                    allVariablesApiModel.nTrueSendForDay = 0;
                    allVariablesApiModel.nFalseSendForDay = 0;
                }

                HttpResponseMessage responseMessage = await httpClient.GetAsync(pathApi);

                if (allVariablesApiModel.lastHour.Count >= 3600)
                {
                    if (allVariablesApiModel.lastHour[0])
                    {
                        allVariablesApiModel.nTrueSendLastHour--;
                    }

                    if (!allVariablesApiModel.lastHour[0])
                    {
                        allVariablesApiModel.nFalseSendLastHour--;
                    }

                    allVariablesApiModel.lastHour.RemoveAt(0);
                }

                if (responseMessage.IsSuccessStatusCode)
                {
                    allVariablesApiModel.lastHour.Add(true);

                    allVariablesApiModel.nTrueSendLastHour++;
                    allVariablesApiModel.nTrueSendForDay++;
                }
                if (!responseMessage.IsSuccessStatusCode)
                {
                    allVariablesApiModel.lastHour.Add(false);

                    allVariablesApiModel.nFalseSendLastHour++;
                    allVariablesApiModel.nFalseSendForDay++;
                }

                allVariablesApiModel.nAllSendLastHour = allVariablesApiModel.lastHour.Count;

                allVariablesApiModel.nAllSendForDay++;
                
            }

            catch
            {

            }
        }          
    }
}
