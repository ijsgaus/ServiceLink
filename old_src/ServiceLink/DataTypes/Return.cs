using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace ServiceLink
{
    public static class Return
    {
        public static ReturnNack Nack = ReturnNack.Default;
        public static ReturnRequeue Requeue = ReturnRequeue.Default;
        public static Return<T> ToReturn<T>(this T value) => value;
    }

    public struct Return<T> : IEquatable<Return<T>>
    {
        public ReturnKind Kind { get; }
        private readonly T _value;
        private readonly Exception _error;

        private Return(ReturnKind kind)
        {
            Kind = kind;
            _value = default(T);
            _error = null;
        }

        public Return([NotNull] Exception error)
        {
            _value = default(T);
            Kind = ReturnKind.Error;
            _error = error ?? throw new ArgumentNullException(nameof(error));
        }
        
        public Return(T value)
        {
            _value = value;
            Kind = ReturnKind.Ack;
            _error = null;
        }
        
        public static implicit operator Return<T> (T value) =>
            new Return<T>(value);
        
        public static implicit operator Return<T>(ReturnNack nack) =>
            new Return<T>(ReturnKind.Nack);
        
        public static implicit operator Return<T>(ReturnRequeue nack) =>
            new Return<T>(ReturnKind.Requeue);
        
        public static implicit operator Return<T>(Exception ex) =>
            new Return<T>(ex);


        public void Match(Action<T> onAck, Action<Exception> onError, Action onNack, Action onRequeue)
        {
            switch (Kind)
            {
                case ReturnKind.Nack:
                    onNack();
                    break;
                case ReturnKind.Ack:
                    onAck(_value);
                    break;
                case ReturnKind.Requeue:
                    onRequeue();
                    break;
                case ReturnKind.Error:
                    onError(_error);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public TResult Match<TResult>(Func<T, TResult> onAck, Func<Exception, TResult> onError, Func<TResult> onNack, Func<TResult> onRequeue)
        {
            switch (Kind)
            {
                case ReturnKind.Nack:
                    return onNack();
                case ReturnKind.Ack:
                    return onAck(_value);
                case ReturnKind.Requeue:
                    return  onRequeue();
                case ReturnKind.Error:
                    return onError(_error);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public bool Equals(Return<T> other)
        {
            return EqualityComparer<T>.Default.Equals(_value, other._value) && Equals(_error, other._error) && Kind == other.Kind;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Return<T> && Equals((Return<T>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = EqualityComparer<T>.Default.GetHashCode(_value);
                hashCode = (hashCode * 397) ^ (_error != null ? _error.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int) Kind;
                return hashCode;
            }
        }

        public static bool operator ==(Return<T> left, Return<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Return<T> left, Return<T> right)
        {
            return !left.Equals(right);
        }
    }
}