using System.Threading.Tasks;

namespace Test_example.Messaging.Sender
{
    public interface IRabbitMqSender
    {
        Task<string> SendToRabbitMqAsync(string queueName, string message, string replyQueueName);
    }
}
