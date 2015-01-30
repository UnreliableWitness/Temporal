using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Temporal.Core
{
    public class CacheKeyGenerator
    {
        public bool TryBuildCacheKey(Expression<Action> method, out string cacheKey)
        {
             var methodCallExp = (MethodCallExpression) method.Body;
             string methodName = methodCallExp.Method.Name;

             var arguments = from arg in ((MethodCallExpression)method.Body).Arguments
                             let argAsObj = Expression.Convert(arg, typeof(object))
                             select Expression.Lambda<Func<object>>(argAsObj, null)
                                              .Compile()();

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
