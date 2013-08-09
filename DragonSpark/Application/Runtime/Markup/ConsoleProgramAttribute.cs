using System;

namespace DragonSpark.Application.Console.Markup
{
    /// <summary>
    /// Used to define the runtime behavior of this program when invoked from the command line.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ConsoleProgramAttribute : Attribute
    {
        /// <summary>
        /// Gets and sets the default Console-Verb that will be execute when no 'switch' is specified on the command line.
        /// </summary>
        public string DefaultVerb { get; set; }

        /// <summary>
        /// Gets and sets the default character used as a 'switch'
        /// </summary>
        public string ArgumentSwitch { get; set; }
    }
}
