using System;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;

namespace ServiceLink
{
    public static class ExpressionExtensions
    {
        [NotNull]
        public static PropertyInfo GetProperty<TOwner, TValue>([NotNull] this Expression<Func<TOwner, TValue>> selector) 
            => TryGetProperty(selector) ?? throw new ArgumentException($"Invalid property selector {selector}");

        [CanBeNull]
        private static PropertyInfo TryGetProperty<TOwner, TValue>([NotNull] Expression<Func<TOwner, TValue>> selector)
        {
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            var memberExpr = selector.Body as MemberExpression;
            var propInfo = memberExpr?.Member as PropertyInfo;
            return propInfo;
        }

        [NotNull]
        public static MethodInfo GetMethod<TOwner, TValue>([NotNull] this Expression<Func<TOwner, Action<TValue>>> selector)
            => TryGetMethod(selector) ?? throw new ArgumentException($"Invalid method selector {selector}");
            

        [CanBeNull]
        private static MethodInfo TryGetMethod<TOwner, TValue>([NotNull] Expression<Func<TOwner, Action<TValue>>> selector)
        {
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            var unary = selector.Body as UnaryExpression;
            var mce = unary?.Operand as MethodCallExpression;
            var ce = mce?.Object as ConstantExpression;
            var mi = ce?.Value as MethodInfo;
            return mi;
        }

        [NotNull]
        public static MethodInfo GetMethod<TOwner, TValue, TResult>([NotNull] this Expression<Func<TOwner, Func<TValue, TResult>>> selector)
            => TryGetMethod(selector) ?? throw new ArgumentException($"Invalid method selector {selector}");
            

        [CanBeNull]
        private static MethodInfo TryGetMethod<TOwner, TValue, TResult>([NotNull] Expression<Func<TOwner, Func<TValue, TResult>>> selector)
        {
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            var unary = selector.Body as UnaryExpression;
            var mce = unary?.Operand as MethodCallExpression;
            var ce = mce?.Object as ConstantExpression;
            var mi = ce?.Value as MethodInfo;
            return mi;
        }
    }
}