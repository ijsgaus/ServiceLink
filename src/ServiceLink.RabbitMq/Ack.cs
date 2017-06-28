using System;

namespace ServiceLink.RabbitMq
{
    internal class Ack<T> : IAck<T>
    {
        public Ack(T message, Action<Ack> confirm)
        {
            Message = message;
            Confirm = confirm;
        }

        public T Message { get; }
        public Action<Ack> Confirm { get; }
    }
}