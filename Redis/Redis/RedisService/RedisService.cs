using StackExchange.Redis;

namespace Redis.RedisService
{
    public class RedisService
    {
        private readonly string _Port;
        private readonly string _Host;

        private ConnectionMultiplexer _redisConnection;
        public IDatabase _db { get; set; }

        public RedisService(IConfiguration config)
        {
            _Port = config["Redis-Docker:Port"];
            _Host = config["Redis-Docker:Host"];
        }

        public void Connect()
        {
            var connectionString = $"{_Host}:{_Port}";
            _redisConnection=ConnectionMultiplexer.Connect(connectionString);
        }

        public IDatabase GetDatabase(int db)
        {
            return _redisConnection.GetDatabase(db);
        }
    }
}
