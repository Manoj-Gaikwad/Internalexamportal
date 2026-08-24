using Internalexamportal.Common.Model;

namespace Internalexamportal.Common.RabbitMQ
{
    public interface IProducer
    {

        void SendNotification(NotificationModel notificationModel);
    }
}
