using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace ServiceLink
{
    public static class TaskExtensions
    {
        public static void LogResult([NotNull] this Task task, [NotNull] ILogger logger, EventId eventId,
            string message, params object[] args)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            task.ContinueWith(tsk =>
            {
                if (tsk.IsFaulted)
                    logger.LogError(eventId, tsk.Exception, message, args);
                else if (tsk.IsCanceled)
                    logger.LogInformation(eventId, $"{message} - CANCELLED", args);
                else
                    logger.LogTrace(eventId, message, args);
            });
        }

        public static void LogResult([NotNull] this Task task, [NotNull] ILogger logger, string message,
            params object[] args)
            => task.LogResult(logger, 0, message, args);

        public static void LogResult([NotNull] this Task task, [NotNull] ILogger logger, EventId eventId)
            => task.LogResult(logger, eventId, eventId.Name);


    }
}