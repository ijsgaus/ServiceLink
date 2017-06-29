using System;

namespace ServiceLink.RabbitMq
{
    internal class Ack<T> : IAck<T>
    {
        public Ack(T message, Action<AckKind> confirm)
        {
            Message = message;
            Confirm = confirm;
        }

        public T Message { get; }
        public Action<AckKind> Confirm { get; }
    }
}