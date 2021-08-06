using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace PingForPC
{
    public class Program
    {
        private static System.Timers.Timer timer;
        private static readonly HttpClient httpClient = new HttpClient();
        private static readonly string path = "PingLog.txt";
        private static readonly string mpce03Api = "http://mpce03.avrora.lan/art?key=39fa302c1a6b40e19020b376c9becb3b&stock=235&device=DeviceName&code=12345&source=4";
        private static readonly string mpce04Api = "http://mpce04.avrora.lan/art?key=39fa302c1a6b40e19020b376c9becb3b&stock=235&device=DeviceName&code=12345&source=4";
        async static Task Main(string[] args)
        {
            Program program = new Program();
            await program.getInfo();    
        }

        public async Task getInfo()
        {
            /*timer = new System.Timers.Timer(1000);
            
            timer.Elapsed += get;
            timer.AutoReset = true;
            timer.Enabled = true;

            Thread.Sleep(-1);*/
            await get();
        }
        
        async public static Task get()
        {
            PingModel mpce03 = getPing("mpce03");
            PingModel mpce04 = getPing("mpce04");
            PingApiModel apimpce03 = await getPingApi(mpce03Api);
            PingApiModel apimpce04 = await getPingApi(mpce04Api);

            writeInFile(mpce03);
            writeInFile(mpce04);
            writeInFileByApi(apimpce03);
            writeInFileByApi(apimpce04);
        }

        private static void writeInFile(PingModel pingModel)
        {
            DateTime dateTime = DateTime.Now;
            int milisecond = dateTime.Millisecond;

            string text = $"{dateTime.ToShortDateString()}|{dateTime.ToLongTimeString()}.{milisecond}|{pingModel.RoundtripTime}|{pingModel.Status}|ttl = {pingModel.Ttl}|ip = {pingModel.Address}\n";

            File.AppendAllText(path, text);
        }

        private static void writeInFileByApi( PingApiModel pingApiModel)
        {
            DateTime dateTime = DateTime.Now;
            int milisecond = dateTime.Millisecond;

            string text = $"{dateTime.ToShortDateString()}|{dateTime.ToLongTimeString()}.{milisecond}|{pingApiModel.Milliseconds}|{pingApiModel.IsSuccessStatusCode}|{pingApiModel.StatusCode}|{pingApiModel.RequestMessage.RequestUri}\n";

            File.AppendAllText(path, text);
        }

        async private static Task<PingApiModel> getPingApi(string api)
        {
            PingApiModel pingApiModel = new PingApiModel();

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            HttpResponseMessage responseMessage = await httpClient.GetAsync(api);
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            pingApiModel.Milliseconds = ts.Milliseconds;
            pingApiModel.StatusCode = responseMessage.StatusCode;
            pingApiModel.IsSuccessStatusCode = responseMessage.IsSuccessStatusCode;
            pingApiModel.RequestMessage = responseMessage.RequestMessage;

            return pingApiModel;
        }

        private static PingModel getPing(string ip)
        {
            PingModel pingModel = new PingModel();

            Ping ping = new Ping();

            PingReply reply = ping.Send(ip);

            pingModel.RoundtripTime = reply.RoundtripTime;

            if (reply.Status == IPStatus.Success)
            {
                pingModel.Status = "True";
            }
            if (reply.Status != IPStatus.Success)
            {
                pingModel.Status = "False";
            }

            if (reply.Options == null)
            {
                pingModel.Ttl = 0;
            }
            else
            {
                pingModel.Ttl = reply.Options.Ttl;
            }

            pingModel.Address = reply.Address;

            return pingModel;
        }
    }
}
