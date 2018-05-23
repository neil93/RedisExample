using StackExchange.Redis;

namespace RedisTest
{
    public class RedisConnectionFactory
    {
        private readonly object _obj = new object();

        private volatile ConnectionMultiplexer _redis;

        public ConnectionMultiplexer GetRedisInstance
        {
            get
            {
                if (_redis==null)
                {
                    lock (_obj)
                    {
                        if (_redis==null || !_redis.IsConnected || !_redis.GetDatabase(0).IsConnected(new RedisKey()))
                        {
                            _redis = ConnectionMultiplexer.Connect("172.16.45.34:9417");
                        }
                    }
                    
                }
                return _redis;
            }
            
        }


    }
}
