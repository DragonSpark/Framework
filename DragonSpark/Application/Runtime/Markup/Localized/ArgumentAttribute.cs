using System;

namespace DragonSpark.Application.Console.Markup.Localized
{
    /// <summary>
    /// Used to mark a method parameter as an argument that can be passed from the command line.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class ArgumentAttribute : Markup.ArgumentAttribute
    {
        public ArgumentAttribute(string name)
            : base(name)
        {
            
        }
    }
}
