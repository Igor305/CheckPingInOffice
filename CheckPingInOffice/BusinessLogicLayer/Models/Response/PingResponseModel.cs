namespace BusinessLogicLayer.Models.Response
{
    public class PingResponseModel
    {
        public double nAllSendLastHour { get; set; }
        public double nTrueSendLastHour { get; set; }
        public double nFalseSendLastHour { get; set; }
        public double percentsLastHour { get; set; }
        public double nAllSendForDay { get; set; }
        public double nTrueSendForDay { get; set; }
        public double nFalseSendForDay { get; set; }
        public double percentsForDay { get; set; }
        public double nAllSendYesterday { get; set; }
        public double nTrueSendYesterday { get; set; }
        public double nFalseSendYesterday { get; set; }
        public double percentsYesterday { get; set; }
    }
}
