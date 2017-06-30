using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.Extensions.Logging;

namespace ServiceLink
{
    public static class LoggerExtensions
    {
        public static T WithLog<T>(this ILogger logger, Func<T> func, EventId eventId,
            [CallerMemberName] string memberName = "")
        {
            logger.LogTrace(eventId, "Enter");
            try
            {
                var result = func();
                logger.LogTrace(eventId, "Exit");
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(eventId, ex, "When executing");
            }
        }
        
        public static ILogger With(this ILogger logger, params (string, object)[] args)
            => new ContextLogger(logger, args.Select(p => new KeyValuePair<string, object>(p.Item1, p.Item2)));

        public static ILogger With(this ILogger logger, string param, object value)
            => logger.With((param, value));

        private class ContextLogger : ILogger
        {
            private readonly IEnumerable<KeyValuePair<string, object>> _context;
            private readonly ILogger _logger;
             

            public ContextLogger(ILogger logger, IEnumerable<KeyValuePair<string, object>> context)
            {
                _logger = logger;
                _context = context;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                using(_logger.BeginScope(_context))
                    _logger.Log(logLevel, eventId, state, exception, formatter);
            }

            public bool IsEnabled(LogLevel logLevel) => _logger.IsEnabled(logLevel);


            public IDisposable BeginScope<TState>(TState state) => _logger.BeginScope(state); 
            

            

            
        }

    }

    
}