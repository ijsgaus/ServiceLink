using System;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;

namespace ServiceLink
{
    public static class ExpressionExtensions
    {
        public static PropertyInfo GetProperty<TOwner, TValue>([NotNull] this Expression<Func<TOwner, TValue>> selector)
        {
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            var memberExpr = selector.Body as MemberExpression;
            var propInfo = memberExpr?.Member as PropertyInfo;
            if(propInfo == null) throw new ArgumentException($"Invalid property selector {selector}");
            return propInfo;
        }
    }
}