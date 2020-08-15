using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Threading;

namespace EventBusRabbitMQ
{
    public class RabbitMqConnection : IRabbitMqConnection
    {
        private readonly IConnectionFactory _connectionFactory;

        private IConnection _connection;
        private bool _disposed;

        public RabbitMqConnection(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;

            if (!IsConnected) TryConnect();
        }

        public bool IsConnected => _connection is { } && _connection.IsOpen && !_disposed;

        public bool TryConnect()
        {
            if (IsConnected) return IsConnected;

            try
            {
                _connection = _connectionFactory.CreateConnection();
            }
            catch (BrokerUnreachableException)
            {
                Thread.Sleep(2000);
                _connection = _connectionFactory.CreateConnection();
            }

            return IsConnected;
        }

        public IModel CreateModel()
        {
            if (!IsConnected) throw new InvalidOperationException("No rabbit connection");

            return _connection.CreateModel();
        }

        public void Dispose()
        {
            if (_disposed) return;

            _connection.Dispose();
            _disposed = _connection.IsOpen;
        }
    }
}
