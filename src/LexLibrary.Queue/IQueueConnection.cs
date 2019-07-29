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

        void Send(object obj);

        void Send(object obj, MessageQueueTransaction mqTransaction);

        void Purge();
    }
}