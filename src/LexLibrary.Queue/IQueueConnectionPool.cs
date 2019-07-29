using System;
using System.Messaging;

namespace LexLibrary.Queue
{
    public interface IQueueConnectionPool : IDisposable
    {
        IQueueConnection Create(string queuePath);
    }
}