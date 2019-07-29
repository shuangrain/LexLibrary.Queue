# LexLibrary.Queue

Encapsulate MSMQ(Microsoft Message Queuing) for easy to use, and minimize queue content with JSON for performance.    
Please let me know if you have any concerns or questions.

![](https://i.imgur.com/r1KTXKZ.png)

# How to use?

* Send Data To Queue
````
string queuePath = @"DESKTOP-2T3QBNE\private$\myqueue";
using (IQueueConnectionPool queueConnectionPool = new QueueConnectionPool())
{
    IQueueConnection queueConnection = queueConnectionPool.Create(queuePath);

    var model = new DataModel
    {
        Name = "https://exfast.me/"
    };
    queueConnection.Send(model);
}
````

* Receive Data From Queue
````
string queuePath = @"DESKTOP-2T3QBNE\private$\myqueue";
using (IQueueConnectionPool queueConnectionPool = new QueueConnectionPool())
{
    IQueueConnection queueConnection = queueConnectionPool.Create(queuePath);

    var model = queueConnection.Receive<DataModel>();
    Console.WriteLine(model.Name);
}
````

* Get Queue Count
````
string queuePath = @"DESKTOP-2T3QBNE\private$\myqueue";
using (IQueueConnectionPool queueConnectionPool = new QueueConnectionPool())
{
    IQueueConnection queueConnection = queueConnectionPool.Create(queuePath);
    Console.WriteLine(queueConnection.Count());
}
````

* Send Data With Transaction To Queue
````
string queuePath = @"DESKTOP-2T3QBNE\private$\myqueue";
using (IQueueConnectionPool queueConnectionPool = new QueueConnectionPool())
using (var mqTransaction = new MessageQueueTransaction())
{
    IQueueConnection queueConnection = queueConnectionPool.Create(queuePath);

    mqTransaction.Begin();

    var model = new DataModel
    {
        Name = "https://exfast.me/"
    };
    queueConnection.Send(model, mqTransaction);

    mqTransaction.Commit();
}
````

* Receive Data With Transaction From Queue
````
string queuePath = @"DESKTOP-2T3QBNE\private$\myqueue";
using (IQueueConnectionPool queueConnectionPool = new QueueConnectionPool())
using (var mqTransaction = new MessageQueueTransaction())
{
    IQueueConnection queueConnection = queueConnectionPool.Create(queuePath);

    mqTransaction.Begin();

    var model = queueConnection.Receive<DataModel>();
    Console.WriteLine(model.Name);

    mqTransaction.Commit();
}
````
