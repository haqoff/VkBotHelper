using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace VkBotHelper.Helper
{
    internal static class CommonHelper
    {
        internal static IEnumerable<KeyValuePair<T, MethodInfo>> ExtractMethodsWithAttribute<T>(Type classExtractFrom)
            where T : Attribute
        {
            var methods = classExtractFrom.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var attrType = typeof(T);

            foreach (var methodInfo in methods)
            {
                var attribute = (T) Attribute.GetCustomAttribute(methodInfo, attrType);
                if (attribute == null) continue;

                yield return new KeyValuePair<T, MethodInfo>(attribute, methodInfo);
            }
        }

        internal static T CreateTypedDelegate<T>(this MethodInfo methodInfo, object target) where T : Delegate
        {
            return (T) methodInfo.CreateDelegate(typeof(T), target);
        }
    }
}