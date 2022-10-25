namespace Presentation.Receive.Tests;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Infrastructure;
using System.Reflection;

public class Tests
{
    [Test]
    public void TestsIntegration()
    {
        // ARRANGE
        var queueName = "MSGID";
        var factory = new ConnectionFactory()
            { HostName = "localhost", UserName="guest", Password="guest" };
        var isOpen = false;
        var endpoint = "amqp://localhost:5672";
        var endPointIntegation = string.Empty;

        // ACT
        using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                endPointIntegation = connection.Endpoint.ToString();
                channel.QueueDeclare(queueName, false, false, false, null);
                var consumer = new EventingBasicConsumer(channel);
                isOpen = channel.IsOpen;

                consumer.Received += (model, ea) =>
                {
                    var bodyBytes = ea.Body.ToArray();
                    var body = Encoding.UTF8.GetString(bodyBytes);
                    var routingKey = ea.RoutingKey;
                     channel.BasicAck(ea.DeliveryTag, false);
                };

                channel.BasicConsume(queueName, true, consumer);
            }

        // ASSERT
        Assert.True(isOpen);
        Assert.AreEqual(endpoint, endPointIntegation);
    }

    [Test]
    public void TestsGetName()
    {
        // I should not test a private method

        // ARRANGE
        var str = "Hello my name is, Cameron";
        var expectedValue = "Cameron";

        MessageHandler target = new MessageHandler();
        var obj =  typeof(MessageHandler)
        .GetMethod("GetName", BindingFlags.NonPublic | BindingFlags.Instance);

        object[] parameters = {str};

        // ACT
        var name = obj?.Invoke(target, parameters);

        // ASSERT
        Assert.AreEqual(name, expectedValue);
    }
}