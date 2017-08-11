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
            var ruleManager = new RuleManager<ICommandSource>();
            Bus<ICommandSource>
                .Configure(ruleManager)
                .Sample.PublishAsync(new SampleEvent());

            Bus<ICommandSource>
                .Configure(ruleManager)
                .Sample.Publish(new SampleEvent());
            Bus<ICommandSource>
                .Configure(ruleManager)
                .Sample.Deliver(store, new SampleEvent()).Only();

            Bus<ICommandSource>
                .Configure(ruleManager)
                .Sample.Deliver(store, new SampleEvent()).After.Using<ISecondStore>(ss => ss.MarkAnsweredMail());


            Service<ICommandSource>(ruleManager).Event(p => p.Sample);
            
            var t = new Test<ICommandSource>();
        
        }
    }
}