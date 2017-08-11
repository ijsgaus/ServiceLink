using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace ServiceLink
{
    public static class Prelude
    {
        public static IService<T> Bus<T>(Configuration config)
        {
            
        }



        public delegate Result<T1> Predicate<in T, T1>(T value);
        
        
        

        public static Predicate<object, Type> IsType = 
        value =>
        {
            var t = value as Type;
            return t != null ? t.ToOk() : new ArgumentException($"{value} is not type");
        };

        public static Predicate<T1, T3> Klein<T1, T2, T3>(this Predicate<T1, T2> f1, Predicate<T2, T3> f2)
            => o => f1(o).Bind(t => f2(t));

        public static Predicate<T1, T3> FunMap<T1, T2, T3>(this Predicate<T1, T2> f1, Func<T2, T3> f2)
            => o => f1(o).Map(t => f2(t));

        public static Predicate<T1, T2> FilterFail<T1, T2>(this Predicate<T1, T2> f1, Func<T2, bool> f2,
            Exception ex)
            => o => f1(o).Bind(t => f2(t) ? t.ToOk() : ex);
        
        
        public static Predicate<Type, TypeInfo> ToTypeInfo =
            IsType.FunMap(p => p.GetTypeInfo());

        public static Predicate<Type, TypeInfo> IsInterface =
            ToTypeInfo.FilterFail(p => p.IsInterface, new ArgumentException($"{p} is not interface"));

        public static Predicate<Type, TA> HasAttribute =
            IsInterface.Klein(p => p.GetCustomAttribute<TA>() as a != null ? a.ToOk() : new ArgumentException());
        
        public static Predicate<MemberInfo, TA>  HasAttribute
            where TA: Attribute
            =

    }


    public interface IPredicate
    {
        Result<bool> IsSubject<T>(T @object);
    }

    public interface IDeriver : IPredicate 
    {
        Result<IReadOnlyCollection<IPredicate>> Derive<T>(IReadOnlyCollection<IPredicate> facts, T @obj);
    }

    
    
    public class Configuration
    {
        private Lst<object> _facts = Lst<object>.Empty;
        private Lst<object> _predicates = Lst<object>.Empty;
    }
}