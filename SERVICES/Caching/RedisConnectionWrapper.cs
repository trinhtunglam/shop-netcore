using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SERVICES.Caching
{
    public class RedisConnectionWrapper : IRedisConnectionWrapper
    {
        private readonly Lazy<string> _connectionString;
        private readonly ConnectionSettings _connectionSettings;

        private volatile ConnectionMultiplexer _connection;
        private readonly object _lock = new object();

        public RedisConnectionWrapper(IOptions<ConnectionSettings> connectionSettings)
        {
            _connectionSettings = connectionSettings.Value;
            _connectionString = new Lazy<string>(GetConnectionString);
        }

        private string GetConnectionString()
        {
            return _connectionSettings.RedisConnection;
        }

        private ConnectionMultiplexer GetConnection()
        {
            if (_connection != null && _connection.IsConnected) return _connection;

            lock (_lock)
            {
                if (_connection != null && _connection.IsConnected) return _connection;

                //Connection disconnected. Disposing connection...
                _connection?.Dispose();

                //Creating new instance of Redis Connection
                _connection = ConnectionMultiplexer.Connect(_connectionString.Value);
            }

            return _connection;
        }

        public IDatabase Database(int? db = null)
        {
            return GetConnection().GetDatabase(db ?? 10); //_settings.DefaultDb);
        }

        public IServer Server(EndPoint endPoint)
        {
            return GetConnection().GetServer(endPoint);
        }

        public EndPoint[] GetEndpoints()
        {
            return GetConnection().GetEndPoints();
        }

        public void FlushDb(int? db = null)
        {
            var endPoints = GetEndpoints();

            foreach (var endPoint in endPoints)
            {
                Server(endPoint).FlushDatabase(db ?? -1); //_settings.DefaultDb);
            }
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}
