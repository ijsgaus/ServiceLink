using System;
using System.Collections.Generic;

namespace ServiceLink
{
    public static class Answer
    {
        public static AnswerNack Nack = AnswerNack.Default;
        public static AnswerRequeue Requeue = AnswerRequeue.Default;
        public static Answer<T> ToAnswer<T>(this T value) => value;
        public static Answer<ValueTuple> Ack = ValueTuple.Create();
    }

    public struct Answer<T> : IEquatable<Answer<T>>
    {
        public AnswerKind Kind { get; }
        private readonly T _value;

        private Answer(AnswerKind kind)
        {
            Kind = kind;
            _value = default(T);
        }
        
        public Answer(T value)
        {
            _value = value;
            Kind = AnswerKind.Ack;
        }
        
        public static implicit operator Answer<T> (T value) =>
            new Answer<T>(value);
        
        public static implicit operator Answer<T>(AnswerNack nack) =>
            new Answer<T>(AnswerKind.Nack);
        
        public static implicit operator Answer<T>(AnswerRequeue nack) =>
            new Answer<T>(AnswerKind.Requeue);


        public void Match(Action<T> onAck, Action onNack, Action onRequeue)
        {
            switch (Kind)
            {
                case AnswerKind.Nack:
                    onNack();
                    break;
                case AnswerKind.Ack:
                    onAck(_value);
                    break;
                case AnswerKind.Requeue:
                    onRequeue();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public TResult Match<TResult>(Func<T, TResult> onAck, Func<TResult> onNack, Func<TResult> onRequeue)
        {
            switch (Kind)
            {
                case AnswerKind.Nack:
                    return onNack();
                case AnswerKind.Ack:
                    return onAck(_value);
                case AnswerKind.Requeue:
                    return  onRequeue();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public bool Equals(Answer<T> other)
        {
            return EqualityComparer<T>.Default.Equals(_value, other._value) && Kind == other.Kind;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Answer<T> && Equals((Answer<T>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (EqualityComparer<T>.Default.GetHashCode(_value) * 397) ^ (int) Kind;
            }
        }

        public static bool operator ==(Answer<T> left, Answer<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Answer<T> left, Answer<T> right)
        {
            return !left.Equals(right);
        }
    }
}