using System;
using System.Collections.Generic;
using System.Text;

namespace SERVICES.Caching
{
    public class ConnectionSettings
    {
        public ConnectionSettings()
        {
            this.RedisConnection = "localhost,allowAdmin=true";
        }

        public string DefaultConnection { get; set; }

        public string MappingConectionString { get; set; }

        public string RedisConnection { get; set; }
    }
}
