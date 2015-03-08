using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Application.Communication.Channels
{
    /// <summary>
    /// Adds extension for the Type type
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// extracts all the methods fromthe given interface type and from all the inherited ones too
        /// </summary>
        /// <param name="type">the type fromwhich extracts all the methods</param>
        /// <returns>list of MemberInfo representing the methods</returns>
        public static IEnumerable<MethodInfo> GetAllInterfaceMethods(this Type type)
        {

            IEnumerable<MethodInfo> methods = type.GetMethods();
            foreach (var subType in type.GetInterfaces())
            {
                methods = methods.Union(GetAllInterfaceMethods(subType));
            }
            return methods;
        }
    }
}
