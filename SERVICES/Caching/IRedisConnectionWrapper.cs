using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using StackExchange.Redis;


namespace SERVICES.Caching
{
    public interface IRedisConnectionWrapper : IDisposable
    {
        IDatabase Database(int? db = null);
        IServer Server(EndPoint endPoint);
        EndPoint[] GetEndpoints();
        void FlushDb(int? db = null);
    }
}
