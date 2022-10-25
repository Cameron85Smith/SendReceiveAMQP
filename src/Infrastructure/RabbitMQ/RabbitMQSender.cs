using System.Text;
using Application.interfaces;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

// 6 This class is responsible for sending a message to the exhange only and therefore, follows SRP.
namespace Infrastructure.RabbitMQ
{
    public class RabbitMQSender : IMessageSender
    {
        // 7 Main entry point to the RabbitMQ client API. Used for constructing IConnection instances.
        private readonly IConnectionFactory _connectionFactory;
        private readonly string _queue;

        // Inject dependencies.
        public RabbitMQSender(IConfiguration configuration)
        {
            //Construct a fresh instance, with all fields set to their respective defaults.
            _connectionFactory = new ConnectionFactory();
            // The uri to use for the connection ("amqp://localhost").
            _connectionFactory.Uri = new Uri(configuration.GetConnectionString("RabbitMQ"));
            // Queue name.
            _queue = configuration["MessageQueue"];
        }

        // 8 Implement the interface. Argument/s is passed from Presentation.Send.
        // This method confirms whether a message was sent to RabbitMQ and therefore, returns a boolean.
        public bool Send(string message)
        {
            bool acks = false;
            
            // Create a connection using an IEndpointResolver.
            using var connection = _connectionFactory.CreateConnection();
            // Common AMQP model.
            using var channel = connection.CreateModel();

            // We set up the detail of channel to communicate with RabbitMQ.
            // (Spec method) Declare a queue.
            channel.QueueDeclare(_queue, false, false, false, null);

            // This event delegate is used when confirmation arrives (channel.BasicAcks).
            channel.BasicAcks += (sender, eventargs) => { acks = true; };

            // Enable publisher acknowledgements.
            channel.ConfirmSelect();

            // Publishes a message to an exchange.
            channel.BasicPublish(string.Empty, _queue, null, Encoding.UTF8.GetBytes(message));

            // Wait until all published messages have been confirmed.
            channel.WaitForConfirmsOrDie();

            return acks;
        }
    }
}