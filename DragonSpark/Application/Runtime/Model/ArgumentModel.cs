using DragonSpark.Application.Console.Markup;
using System;
using System.Reflection;
using ConsoleProgramAttribute = DragonSpark.Application.Console.Markup.Localized.ConsoleProgramAttribute;

namespace DragonSpark.Application.Console.Model
{
    /// <summary>
    /// Represents a program argument at runtime.
    /// </summary>
    internal class ArgumentModel
    {
        public ProgramModel Program
        {
            get;
            private set;
        }

        public ParameterInfo Parameter
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public object Default
        {
            get;
            set;
        }

        public string ParameterName
        {
            get
            {
                return Parameter.Name;
            }
        }

        public VerbModel Verb
        {
            get;
            private set;
        }

        internal object RuntimeValue
        {
            get;
            set;
        }

        public ArgumentModel(ProgramModel program, VerbModel verb, ParameterInfo parameter)
            : this(program, verb, parameter, verb.Program.ArgumentSwitch + parameter.Name)
        {
        }

        public ArgumentModel(ProgramModel program, VerbModel verb, ParameterInfo parameter, ArgumentAttribute attribute)
            : this(program, verb, parameter, attribute.Name)
        {
            Description = attribute is Markup.Localized.ArgumentAttribute ? ConsoleProgramAttribute.GetResourceString( program.ProgramType, attribute.Description ) : attribute.Description;

            if (attribute.Default != null && !parameter.ParameterType.IsInstanceOfType(attribute.Default))
                throw new ArgumentException(string.Format("Cannot use a default value of type {0} for argument <{1}> of type {2}", attribute.Default.GetType().FullName, this.Name, parameter.ParameterType.FullName));

            Default = attribute.Default;
        }

        public ArgumentModel(ProgramModel program, VerbModel verb, ParameterInfo parameter, string name)
        {
            this.Program = program;
            this.Verb = verb;
            this.Parameter = parameter;
            this.Name = name;
        }
    }
}
