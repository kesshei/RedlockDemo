using Medallion.Threading.MySql;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MysqlLockDemo
{
    class Program
    {
static void Main(string[] args)
{
    Console.Title = "MySql Lock Demo by 蓝创精英团队";
    Test();
    Console.WriteLine("MySql Lock  案例!");
    Console.ReadLine();
}
public static void Test()
{
    var tasks = new List<Task>();
    for (int i = 0; i < 5; i++)
    {
        tasks.Add(Task.Run(async () =>
        {
            var ID = Guid.NewGuid().ToString("N");
            var @lock = new MySqlDistributedLock("key", "Server=127.0.0.1;Port=3306;Database=test;Uid=root;Pwd=123456;");
            await using (await @lock.AcquireAsync(TimeSpan.FromSeconds(15 * 10)))
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
