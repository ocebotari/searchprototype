using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SimonsVossSearchPrototype.DAL
{
    internal static class Extensions
    {
        internal static bool FullTextSearch(dynamic source, string text, bool caseSensitive = false)
        {
            var compareFunc = caseSensitive
                           ? new Func<string, string, bool>((a, b) => a.IndexOf(b, StringComparison.Ordinal) >= 0)
                           : new Func<string, string, bool>((a, b) => a.IndexOf(b, StringComparison.OrdinalIgnoreCase) >= 0);

            bool AnyPropertyHasValue(dynamic current)
            {
                if (current == null)
                    return false;

                if (IsValueReferenceType(current.GetType()))
                {
                    foreach (var srcProp in GetProperties(current))
                    {
                        if (IsEnumerable(srcProp.PropertyType) && srcProp.PropertyType != typeof(ExpandoObject))
                        {
                            foreach (var i in GetValue(current, srcProp) as IEnumerable)
                            {
                                if (AnyPropertyHasValue(i))
                                    return true;
                            }
                        }
                        else
                        {
                            if (AnyPropertyHasValue(GetValue(current, srcProp)))
                                return true;
                        }
                    }
                }
                else
                {
                    if (compareFunc(current.ToString(), text))
                        return true;
                }

                return false;
            }

            return AnyPropertyHasValue(source);
        }

        private static bool IsValueReferenceType(dynamic type)
        {
            return !type.IsValueType && !type.IsPrimitive && type != typeof(string);
        }

        private static object GetValue(object source, dynamic srcProp)
        {
            return source is ExpandoObject ? srcProp.Value : srcProp.GetValue(source, null);
        }

        private static IEnumerable<dynamic> GetProperties(object source)
        {
            if (source is ExpandoObject)
            {
                return ((IDictionary<string, object>)source)
                    .Select(i => new { Name = i.Key, Value = i.Value, PropertyType = i.Value?.GetType() })
                    .ToList();
            }
            else
            {
                return source.GetType().GetProperties();
            }
        }

        private static bool IsEnumerable(Type toTest)
        {
            return typeof(IEnumerable).IsAssignableFrom(toTest) && toTest != typeof(string);
        }

        public static bool IsMatch(this string input, string textToMatch)
        {
            if (string.IsNullOrWhiteSpace(input)) return false;

            var regex = new Regex(textToMatch, RegexOptions.IgnoreCase);

            return regex.IsMatch(input);
        }
    }
}
