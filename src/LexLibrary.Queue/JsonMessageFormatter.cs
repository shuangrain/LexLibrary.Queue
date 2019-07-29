using System;
using System.IO;
using System.Messaging;
using System.Text;
using Newtonsoft.Json;

namespace LexLibrary.Queue
{
    /// <summary>
    /// ref: https://gist.github.com/jchadwick/2430984
    /// </summary>
    public class JsonMessageFormatter : IMessageFormatter
    {
        public bool CanRead(Message message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            Stream stream = message.BodyStream;

            return ((stream != null) && (stream.CanRead) && (stream.Length > 0));
        }

        public object Clone()
        {
            return new JsonMessageFormatter();
        }

        public object Read(Message message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (!CanRead(message))
            {
                return null;
            }

            using (var sr = new StreamReader(message.BodyStream, Encoding.UTF8))
            {
                var json = sr.ReadToEnd();
                return JsonConvert.DeserializeObject(json);
            }
        }

        public void Write(Message message, object obj)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            string json = JsonConvert.SerializeObject(obj);

            message.BodyStream = new MemoryStream(Encoding.UTF8.GetBytes(json));

            //Need to reset the body type, in case the same message
            //is reused by some other formatter.
            message.BodyType = 0;
        }
    }
}