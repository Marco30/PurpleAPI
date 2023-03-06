using RabbitMQ.Client;

// RabbitMQService is a class that wraps the RabbitMQ client library
// and provides methods for creating connections, channels, and interacting
// with queues and exchanges.

namespace PurpleAPI.Microservice
{
    public class RabbitMQService
    {
        private readonly IConnection _connection; // An instance of IConnection, which represents a connection to a RabbitMQ broker.
        private readonly IModel _channel; // An instance of IModel, which represents a channel for communicating with RabbitMQ.

        public RabbitMQService()
        {
            string hostname = "localhost";
            string username = "username";
            string password = "password";
            string virtualhost = "/";

            var factory = new ConnectionFactory // Create a new instance of ConnectionFactory, which is provided by the RabbitMQ client library.
            {
                HostName = hostname, // Set the hostname of the RabbitMQ broker.
                UserName = username, // Set the username to use when connecting to the broker.
                Password = password, // Set the password to use when connecting to the broker.
                VirtualHost = virtualhost
            };

            _connection = factory.CreateConnection(); // Create a new connection using the settings provided in the ConnectionFactory.
            _channel = _connection.CreateModel(); // Create a new channel using the connection.

            // Declare queues and exchanges here
            _channel.QueueDeclare(queue: "pdf-generation", durable: true, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueDeclare(queue: "storage", durable: true, exclusive: false, autoDelete: false, arguments: null);
            // Declare a queue named "my-queue" with durable: true, exclusive: false, autoDelete: false, and no additional arguments.
        }

        /* public void Dispose()
        {
            _channel.Close(); // Close the channel.
            _connection.Close(); // Close the connection.
        } */

        // Add this public method to return the channel instance
        public IModel GetChannel()
        {
            return _channel;
        }

    }
}
