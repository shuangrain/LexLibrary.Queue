using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Messaging;

namespace LexLibrary.Queue
{
    public class QueueConnection : IQueueConnection
    {
        private readonly MessageQueue _messageQueue = null;

        public QueueConnection(string queuePath)
        {
            _messageQueue = new MessageQueue(queuePath);
            _messageQueue.Formatter = new JsonMessageFormatter();
        }

        /// <summary>
        /// ref: https://stackoverflow.com/questions/3869022/is-there-a-way-to-check-how-many-messages-are-in-a-msmq-queue
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            using (var queueCounter = new PerformanceCounter("MSMQ Queue", "Messages in Queue", _messageQueue.Path))
            {
                return (int)queueCounter.NextValue();
            }
        }

        public T Peek<T>()
        {
            Message msg = _messageQueue.Peek();
            JToken jToken = msg?.Body as JToken;
            return jToken.ToObject<T>();
        }

        public T Receive<T>()
        {
            Message msg = _messageQueue.Receive();
            JToken jToken = msg?.Body as JToken;
            return jToken.ToObject<T>();
        }

        public T Receive<T>(MessageQueueTransaction mqTransaction)
        {
            Message msg = _messageQueue.Receive(mqTransaction);
            JToken jToken = msg?.Body as JToken;
            return jToken.ToObject<T>();
        }

        public void Send(object obj, MessagePriority priority)
        {
            using (Message msg = new Message(obj, _messageQueue.Formatter))
            {
                msg.Priority = priority;
                _messageQueue.Send(msg);
            }
        }

        public void Send(object obj, MessagePriority priority, MessageQueueTransaction mqTransaction)
        {
            using (Message msg = new Message(obj, _messageQueue.Formatter))
            {
                msg.Priority = priority;
                _messageQueue.Send(msg, mqTransaction);
            }
        }

        public void CreateIfNotExists(bool transactional)
        {
            if (!MessageQueue.Exists(_messageQueue.Path))
            {
                MessageQueue.Create(_messageQueue.Path, transactional);
            }
        }

        public void Purge()
        {
            _messageQueue.Purge();
        }

        public void Dispose()
        {
            _messageQueue.Dispose();
        }
    }
}
