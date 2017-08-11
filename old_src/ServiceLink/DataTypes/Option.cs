using System;

namespace ServiceLink
{
    public struct Option<T> : IEquatable<Option<T>>
    {
        public Option(T value) : this()
        {
            _data = (true, value);
        }

        private readonly (bool, T) _data;

        public bool IsSome => _data.Item1;
        public bool IsNone => !IsSome;

        public static implicit operator Option<T>(OptionNone none) => new Option<T>();
        public static implicit operator Option<T>(T obj) => new Option<T>(obj);

        public TResult Match<TResult>(Func<T, TResult> omSome, Func<TResult> onNone)
            => IsSome ? omSome(_data.Item2) : onNone();
        
        public void Match(Action<T> omSome, Action onNone)
        {
            if (IsSome) omSome(_data.Item2); else onNone();
        }


        public bool Equals(Option<T> other)
        {
            return _data.Equals(other._data);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Option<T> && Equals((Option<T>) obj);
        }

        public override int GetHashCode()
        {
            return _data.GetHashCode();
        }

        public static bool operator ==(Option<T> left, Option<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Option<T> left, Option<T> right)
        {
            return !left.Equals(right);
        }
    }
    
    public struct OptionNone
    {
    }

    public class Option
    {
        public static OptionNone None = new OptionNone();
        
    }
}