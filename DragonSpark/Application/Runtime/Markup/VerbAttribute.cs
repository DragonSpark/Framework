using System;

namespace DragonSpark.Application.Console.Markup
{
    /// <summary>
    /// Used to mark a method as a 'verb', an action that can be invoked from the command line.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class VerbAttribute : Attribute
    {
        /// <summary>
        /// Gets and sets the name of this ConsoleVerb.
        /// This value is used to reference this verb, for example, as the ConsoleProgram's DefaultVerb.
        /// </summary>
        public virtual string VerbName
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets and sets the description for this ConsoleVerb.
        /// This value will appear in the program help output.
        /// </summary>
        public virtual string Description
        {
            get;
            set;
        }

        public VerbAttribute(string verbname)
        {
            VerbName = verbname;
        }
    }
}