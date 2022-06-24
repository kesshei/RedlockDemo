using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RedlockDemo
{
class Program
{
    static void Main(string[] args)
    {
        Console.Title = "redis redlock Demo by 蓝创精英团队";
        var connMultiplexer = ConnectionMultiplexer.Connect("127.0.0.1");
        Console.WriteLine($"redis连接状态:{connMultiplexer.IsConnected}");
        var multiplexers = new List<RedLockMultiplexer> { connMultiplexer };
        using var redlockFactory = RedLockFactory.Create(multiplexers);
        Test(redlockFactory);
        Console.WriteLine("redis redlock  案例!");
        Console.ReadLine();
    }
    public static void Test(RedLockFactory RedLockFactory)
    {
        var tasks = new List<Task>();
        for (int i = 0; i < 5; i++)
        {
            tasks.Add(Task.Run(() =>
            {
                var ID = Guid.NewGuid().ToString("N");
                using var redislock = RedLockFactory.CreateLock("key", TimeSpan.FromSeconds(15), TimeSpan.FromSeconds(15 * 10), TimeSpan.FromSeconds(1));
                if (redislock.IsAcquired)
                {
                    Console.WriteLine($"{DateTime.Now} - {ID}:申请到标志位!");
                    Console.WriteLine($"{DateTime.Now} - {ID}:处理自己的 事情!");
                    Thread.Sleep(5 * 1000);
                    Console.WriteLine($"{DateTime.Now} - {ID}:处理完毕!");
                }
            }));
        }
        Task.WaitAll(tasks.ToArray());
    }
}
}
