using System;
using System.Linq.Expressions;
using System.Reflection;
using Contracts;

namespace MicroService1
{

    public class Test<T>
    {
        public EPoint<TMessage, TAnswer> EPoint<TMessage, TAnswer>(
            Expression<Func<T, Func<TMessage, TAnswer>>> selector)
        {
            return new EPoint<TMessage, TAnswer>();
        }
    }

    public class EPoint<TMessage, TAnswer>
    {
        
    }
    
    
    class Program
    {
        
        
        static void Main(string[] args)
        {
            var t = new Test<ICommandSource>();
        
        }
    }
}