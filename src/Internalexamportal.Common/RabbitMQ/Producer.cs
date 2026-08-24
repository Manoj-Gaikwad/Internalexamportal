using Internalexamportal.Common.Model;
using Internalexamportal.Common.RabbitMQ.Model;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Internalexamportal.Common.RabbitMQ
{
    public class Producer : IProducer
    {

        private readonly RabbitMQCredentials _rabitMQCreds;


        public Producer(IOptions<RabbitMQCredentials> rabitMQCreds)
        {
            _rabitMQCreds = rabitMQCreds.Value;
        }

        public void SendNotification(NotificationModel notificationModel)
        {
            ConnectionFactory factory = new ConnectionFactory
            {
                UserName = _rabitMQCreds.UserName,
                Password = _rabitMQCreds.Password,
                VirtualHost = _rabitMQCreds.VirtualHost,
                HostName = _rabitMQCreds.HostName
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: _rabitMQCreds.Queue,
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

                var msgJsonString = JsonConvert.SerializeObject(notificationModel);
                var body = Encoding.UTF8.GetBytes(msgJsonString);

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                channel.BasicPublish(exchange: "",
                                     routingKey: _rabitMQCreds.Queue,
                                     basicProperties: properties,
                                     body: body);
            }
        }
    }
}
