using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Temporal.Core
{
    public class CacheKeyGenerator
    {
        public bool TryBuildCacheKey(MethodInfo method, IEnumerable<object> arguments, out string cacheKey)
        {
            var methodName = method.Name;

            string tempCacheKey;
            var result = TryBuildCacheKey(methodName, arguments, out tempCacheKey);
            cacheKey = tempCacheKey;
            return result;
        }

        public bool TryBuildCacheKey(Expression<Action> method, out string cacheKey)
        {
             var methodCallExp = (MethodCallExpression) method.Body;
             string methodName = methodCallExp.Method.Name;

             var arguments = from arg in ((MethodCallExpression)method.Body).Arguments
                             let argAsObj = Expression.Convert(arg, typeof(object))
                             select Expression.Lambda<Func<object>>(argAsObj, null)
                                              .Compile()();

            string tempCacheKey;
            var result = TryBuildCacheKey(methodName, arguments, out tempCacheKey);
            cacheKey = tempCacheKey;
            return result;
        }

        private bool TryBuildCacheKey(string methodName, IEnumerable<object> arguments, out string cacheKey)
        {
            var sb = new StringBuilder();
            foreach (var argument in arguments)
            {
                var argKey = new StringBuilder();
                if (argument is string || argument is int)
                    argKey.Append(argument);
                else if (argument is IEnumerable<int>)
                {
                    var list = argument as IEnumerable<int>;
                    foreach (var i in list)
                    {
                        argKey.AppendFormat("${0}", i);
                    }
                }
                else if (argument is IEnumerable<string>)
                {
                    var list = argument as IEnumerable<string>;
                    foreach (var i in list)
                    {
                        argKey.AppendFormat("${0}", i);
                    }
                }
                else if (argument == null)
                    argKey.Append("null");
                else if (argument is bool)
                    argKey.Append(argument);
                else if (argument is Enum)
                    argKey.Append(argument);
                else
                {
                    cacheKey = string.Empty;
                    return false;
                }
                sb.Append("#" + argKey);
            }

            cacheKey = methodName + sb;

            return true;
        }
    }
}
