using System;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.Extensions.Logging;

namespace ServiceLink
{
    public static class LoggerExtensions
    {
        public static T WithLog<T>(this ILogger logger, Func<T> func, string message, params object[] parameters)

        {
            using (logger.BeginScope(message, parameters))
            {
                try
                {
                    logger.LogTrace("Enter");
                    var result = func();
                    logger.LogTrace("Exit");
                    return result;
                }
                catch (Exception ex)
                {
                    logger.LogError(0, ex, "Error");
                    throw;
                }
            }
        }

        
    }
}