using System;
using System.Messaging;

namespace LexLibrary.Queue
{
    public interface IQueueConnection : IDisposable
    {
        int Count();

        void CreateIfNotExists(bool transactional);

        T Peek<T>();

        T Receive<T>();

        T Receive<T>(MessageQueueTransaction mqTransaction);

        void Send(object obj, MessagePriority priority = MessagePriority.Normal);

        void Send(object obj, MessagePriority priority, MessageQueueTransaction mqTransaction);

        void Purge();
    }
}