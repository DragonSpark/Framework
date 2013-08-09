using System;

namespace DragonSpark.Application.Console.Markup.Localized
{
    /// <summary>
    /// Used to mark a method as a 'verb', an action that can be invoked from the command line.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class VerbAttribute : Markup.VerbAttribute
    {
        public VerbAttribute(string verbname)
            : base(verbname)
        {
            
        }
    }
}
