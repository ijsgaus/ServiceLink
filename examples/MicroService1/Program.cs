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
        static void Run<TService, TMessage>(Expression<Func<TService, Action<TMessage>>> expression)
        {
            var name =
            ((((expression.Body as UnaryExpression).Operand as MethodCallExpression).Object as ConstantExpression)
                .Value as MethodInfo).Name;
        }
        
        static void Main(string[] args)
        {
            var t = new Test<ICommandSource>();
            t.EPoint(p => p.SampleWithAnswer);
            Run<ICommandSource, Command>(p => p.Execute);
            Console.WriteLine("Hello World!");
        }
    }
}