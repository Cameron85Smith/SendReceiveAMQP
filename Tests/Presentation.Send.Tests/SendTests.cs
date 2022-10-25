using Moq;
using RabbitMQ.Client;
using System.Text;

namespace Tests;

public class SendTests
{
    [Test]
    public void SendToRabbitMQ()
    {
        bool acks = false;
        var message = "Hello my name is, Cameron";
        var queueName = "MSGID";

        IConnectionFactory factory = new ConnectionFactory()
            { HostName = "localhost", UserName="guest", Password="guest" };

        using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queueName, false, false, false, null);
                channel.BasicAcks += (sender, eventargs) => { acks = true; };
                channel.ConfirmSelect();

                channel.BasicPublish(string.Empty,
                                        queueName,
                                        null,
                                        Encoding.UTF8.GetBytes(message));

                channel.WaitForConfirmsOrDie();
        }

        Assert.True(acks);
    }
}