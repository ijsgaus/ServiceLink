using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ServiceLink
{
    public static partial class Extensions
    {
        public static T WithSide<T>(this T value, Action action)
        {
            action();
            return value;
        }

        public static T WithSide<T>(this T value, Action<T> action)
        {
            action(value);
            return value;
        }

        public static TResult Lock<T, TResult>(this T locker, Func<T, TResult> calc)
            where T : class
        {
            lock (locker)
            {
                return calc(locker);
            }
        }

        public static TResult Lock<T, TResult>(this T locker, Func<TResult> calc)
            where T : class
        {
            return Lock(locker, _ => calc());
        }

        public static void Lock<T>(this T locker, Action<T> calc)
            where T : class
        {
            lock (locker)
            {
                calc(locker);
            }
        }

        public static void Lock<T>(this T locker, Action calc)
            where T : class
        {
            Lock(locker, _ => calc());
        }

        public static void LogActivity(this ILogger logger, Action action, string message, params object[] args)
        {
            try
            {
                logger.LogTrace("Starting - " + message, args);
                action();
                logger.LogTrace("Success - " + message, args);
            }
            catch (Exception ex)
            {
                logger.LogError(0, ex, "Error - " + message, args);
                throw;
            }
        }

        public static T LogActivity<T>(this ILogger logger, Func<T> func, string message, params object[] args)
        {
            try
            {
                logger.LogTrace("Starting - " + message, args);
                var result = func();
                logger.LogTrace("Success - " + message, args);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(0, ex, "Error - " + message, args);
                throw;
            }
        }

        public static void Iif(Func<bool> condition, Action whenTrue, Action whenFalse)
        {
            if (condition())
                whenTrue();
            else
                whenFalse();
        }

        public static void Iif<T>(this T value, Func<T, bool> condition, Action<T> whenTrue, Action<T> whenFalse)
        {
            if (condition(value))
                whenTrue(value);
            else
                whenFalse(value);
        }
        
        public static Task LogResult(this Task task, ILogger logger, string msg, params object[] prms)
        {
            if (task.IsFaulted)
                logger.LogError(0, task.Exception, msg + " - Error", prms);
            else if (task.IsCanceled)
                logger.LogInformation(msg + " - Cancelled", prms);
            else
                logger.LogTrace(msg + " - Success", prms);
            return task;
        }

        public static PropertyInfo GetProperty<TOwner, TValue>(this Expression<Func<TOwner, TValue>> selector)
        {
            return TryGetProperty(selector) ?? throw new ArgumentException($"Invalid property selector {selector}");
        }

        private static PropertyInfo TryGetProperty<TOwner, TValue>(Expression<Func<TOwner, TValue>> selector)
        {
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            var memberExpr = selector.Body as MemberExpression;
            var propInfo = memberExpr?.Member as PropertyInfo;
            return propInfo;
        }
        
        // Null monad

        public static TResult NotNullMap<T, TResult>(this T obj, Func<T, TResult> mapper)
            => Equals(obj, null) ? default(TResult) : mapper(obj);

        public static void NotNullDo<T>(this T obj, Action<T> doer)
        {
            if (!Equals(obj, null))
                doer(obj);
        }

        public static string ToCamelCase(this string str)
        {
            if (str == null) return null;
            if (str.Length == 0) return str;
            if (!Char.IsUpper(str, 0)) return str;
            return Char.ToLower(str[0]) + str.Substring(1);
        }

        public static string ToDottedName(this string pascalCaseName, bool stripFirstI)
        {
            var builder = new StringBuilder();
            var first = true;
            foreach (var letter in pascalCaseName)
            {
                if(first && stripFirstI && letter == 'I')
                    continue;
                if (char.IsUpper(letter))
                {
                    if (!first) builder.Append('.');
                    builder.Append(char.ToLower(letter));
                }
                else
                    builder.Append(letter);
                first = false;
            }
            return builder.ToString();

        }
    }
}