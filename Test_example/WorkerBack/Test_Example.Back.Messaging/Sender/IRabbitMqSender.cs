using System.Threading.Tasks;

namespace Test_Example.Back.Messaging.Sender
{
    public interface IRabbitMqSender
    {
        Task SendToRabbitMqAsync(string queueName, string message);
    }
}
