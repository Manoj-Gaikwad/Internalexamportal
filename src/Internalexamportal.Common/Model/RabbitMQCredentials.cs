namespace Internalexamportal.Common.RabbitMQ.Model
{
    public class RabbitMQCredentials
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string VirtualHost { get; set; }
        public string HostName { get; set; }
        public string Queue { get; set; }
    }
}
