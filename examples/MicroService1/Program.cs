using System;
using System.Linq.Expressions;
using System.Reflection;
using Contracts;

namespace MicroService1
{
     
    
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
            Run<ICommandSource, Command>(p => p.Exec);
            Console.WriteLine("Hello World!");
        }
    }
}