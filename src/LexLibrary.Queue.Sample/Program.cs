using System;
using System.Linq;
using System.Messaging;
using System.Threading;

namespace LexLibrary.Queue.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            string mode = args[0];
            string queuePath = args[1];

            if (mode == "s")
            {
                Sender(queuePath);
            }
            else if (mode == "r")
            {
                Receiver(queuePath);
            }

            Console.WriteLine("done !!");
            Console.ReadKey();
        }

        static void Sender(string queuePath)
        {
            using (var queueConnectionPool = new QueueConnectionPool())
            using (var mqTransaction = new MessageQueueTransaction())
            {
                var queueConnection = queueConnectionPool.Create(queuePath);

                queueConnection.CreateIfNotExists(true);

                mqTransaction.Begin();

                foreach (var item in Enumerable.Range(1, 1000))
                {
                    Console.WriteLine($"Add Data: {item}/1000");
                    queueConnection.Send(new DataModel
                    {
                        Name = $"Data - {item}"
                    }, mqTransaction);
                }

                mqTransaction.Commit();
            }
        }

        static void Receiver(string queuePath)
        {
            SemaphoreSlim semaphoreSlim = new SemaphoreSlim(Environment.ProcessorCount);

            using (var queueConnectionPool = new QueueConnectionPool())
            {
                while (true)
                {
                    var queueConnection = queueConnectionPool.Create(queuePath);
                    var model = queueConnection.Receive<DataModel>();

                    semaphoreSlim.Wait();
                    ThreadPool.QueueUserWorkItem((state) =>
                    {
                        int count = queueConnection.Count();
                        Console.WriteLine($"當前: {model.Name}, 剩餘: {count}");
                        Thread.Sleep(1000);

                        semaphoreSlim.Release();
                    });
                }
            }
        }
    }
}
