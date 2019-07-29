using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace LexLibrary.Queue
{
    public class QueueConnectionPool : IQueueConnectionPool
    {
        private readonly ConcurrentDictionary<string, IQueueConnection> _connectionPool =
                            new ConcurrentDictionary<string, IQueueConnection>();

        public IQueueConnection Create(string queuePath)
        {
            if (!_connectionPool.ContainsKey(queuePath))
            {
                _connectionPool.TryAdd(queuePath, new QueueConnection(queuePath));
            }

            return _connectionPool[queuePath];
        }

        public void Dispose()
        {
            foreach (var item in _connectionPool)
            {
                item.Value.Dispose();
            }

            _connectionPool.Clear();
        }
    }
}
