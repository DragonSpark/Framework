using System;

namespace DragonSpark.Application.Console.Markup
{
    /// <summary>
    /// Used to mark a method parameter as an argument that can be passed from the command line.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class ArgumentAttribute : Attribute
    {
        /// <summary>
        /// Gets and sets the name of this ConsoleArgument.
        /// This value is used to reference this argument, for example, as the ConsoleVerb's DefaultArgument.
        /// </summary>
        public virtual string Name { get; private set; }

        /// <summary>
        /// Gets and sets the description for this Console Argument.
        /// This value will appear in the program help output.
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Gets and sets the default value for this Console Argument.
        /// Specifying a default allows this argument to be optional on the command line.
        /// </summary>
        public object Default { get; set; }

        public ArgumentAttribute(string name)
        {
            Name = name;
        }
    }
}
