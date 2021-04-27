using System;
using System.Net.NetworkInformation;

namespace CheckPing
{
    public class Program
    {

        public static void Main(string[] args)
        {
            string result1 = getPing("a0005-router");
            string result2 = getPing("a0005-s");
            string result3 = getPing("188.163.35.276");
            Console.WriteLine(result1+"\n"+ result2 + "\n" + result3);

        } 

        private static string getPing(string ip)
        {
            Ping ping = new Ping();
            string result;
            try
            {
                PingReply reply = ping.Send(ip);
                bool pingable = reply.Status == IPStatus.Success;
                result = "Ваш ответ " + pingable;
            }
            catch
            {
                result = "Боюсь Вы указали неверный ip :(";
            }
            return result;
        }
    }
}