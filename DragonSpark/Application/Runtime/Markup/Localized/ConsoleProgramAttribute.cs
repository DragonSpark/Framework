using System;

namespace DragonSpark.Application.Console.Markup.Localized
{
    /// <summary>
    /// Used to define the runtime behavior of this program when invoked from the command line.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ConsoleProgramAttribute : Markup.ConsoleProgramAttribute
    {
        internal static Type GetResourcesType(Type programType)
        {
            if (!programType.IsDefined(typeof(ConsoleProgramAttribute), false))
                return null;
            return (programType.GetCustomAttributes(typeof(ConsoleProgramAttribute), false)[0] as ConsoleProgramAttribute)._resourceType;
        }

        internal static string GetResourceString(Type programType, string resourceName)
        {
            if (String.IsNullOrEmpty(resourceName))
                return null;
            Type resourceType = ConsoleProgramAttribute.GetResourcesType(programType);
            if (resourceType == null)
                return null;
            return new System.Resources.ResourceManager(resourceType).GetString(resourceName);
        }

        public ConsoleProgramAttribute(Type resourceType)
        {
            _resourceType = resourceType;
        }

        private Type _resourceType;
    }
}
