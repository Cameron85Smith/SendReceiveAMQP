using RabbitMQ.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client.Events;
using System.Text;

// 13 This class is responsible for receiving a message from the exhange only and therefore, follows SRP.
public class RabbitMQReceiver : IMessageReceiver, IHostedService
    {
        // 14 Main entry point of the RabbitMQ client API. Used for constructing IConnection instances.
        private readonly IConnectionFactory _connectionFactory;
        private IConnection? _connection;
        private IModel? _channel;
        private readonly string _queue;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        // Dependency Injection
        public RabbitMQReceiver(IConfiguration configuration, IServiceScopeFactory serviceScopeFactory)
        {
            //Construct a fresh instance, with all fields set to their respective defaults.
            _connectionFactory = new ConnectionFactory();
            // The uri to use for the connection ("amqp://localhost").
            _connectionFactory.Uri = new Uri(configuration.GetConnectionString("RabbitMQ"));
            // Queue name.
            _queue = configuration["MessageQueue"];
            // IServiceScopeFactory's lifetime is always Singleton.
            _serviceScopeFactory = serviceScopeFactory;
        }

        // 15 Implement the interface. Receive from the exchange.
        public void Start()
        {
            // We set up the detail of channel to communicate with RabbitMQ.
            // (Spec method) Declare a queue.
            _channel!.QueueDeclare(_queue, false, false, false, null);
            var consumer = new EventingBasicConsumer(_channel);

            // Event fired when a delivery arrives for the consumer.
            consumer.Received += (model, ea) =>
            {
                var bodyBytes = ea.Body.ToArray();
                var body = Encoding.UTF8.GetString(bodyBytes);
                // Create an IServiceScope which contains an IServiceProvider used to resolve dependencies from a newly created scope.
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    // IMessageHandler is fresh in this regard
                    var messageHandler = scope.ServiceProvider.GetRequiredService<IMessageHandler>();
                    messageHandler.Handle(body);
                }
            };

             // Consumes a message from the exchange.
            _channel.BasicConsume(_queue, true, consumer);
        }

        // Contains the logic to start the background task.
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
            Start();
            return Task.CompletedTask;
        }

        // Contains the logic to end the background task.
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _channel?.Dispose();
            _connection?.Dispose();
            return Task.CompletedTask;
        }
    }