using DragonSpark.Application.Console.Markup;
using System.Collections.Generic;
using System.Reflection;
using ConsoleProgramAttribute = DragonSpark.Application.Console.Markup.Localized.ConsoleProgramAttribute;

namespace DragonSpark.Application.Console.Model
{
    /// <summary>
    /// Represents a program verb at runtime.
    /// </summary>
    internal class VerbModel
    {
        /// <summary>
        /// Gets the name of the verb, used to reference a program method on the command line.
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the MethodInfo that this verb will invoke.
        /// </summary>
        public MethodInfo Method
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the description for this verb that will be displayed in help output.
        /// </summary>
        public string Description
        {
            get;
            private set;
        }

        public IList<ArgumentModel> Arguments
        {
            get;
            internal set;
        }

        public ProgramModel Program
        {
            get;
            private set;
        }

        internal VerbModel(ProgramModel program, MethodInfo method)
            : this(program, method, "/" + method.Name)
        {
        }

        internal VerbModel(ProgramModel program, MethodInfo method, VerbAttribute attribute)
            : this(program, method, attribute.VerbName)
        {
            Description = attribute is Markup.Localized.VerbAttribute ? ConsoleProgramAttribute.GetResourceString( program.ProgramType, attribute.Description ) : attribute.Description;
        }

        internal VerbModel(ProgramModel program, MethodInfo method, string name)
        {
            this.Program = program;
            this.Method = method;
            this.Name = name;

            this.Arguments = new List<ArgumentModel>();

            // create arguments
            BuildArguments();

            //VerbAttribute verbAttr = ConsoleExtensions.GetSingleAttribute<VerbAttribute>(method);
            //if (verbAttr != null)
            //{
            //    if (!string.IsNullOrEmpty(verbAttr.DefaultArgument))
            //        DefaultArgument = (from a in Arguments where a.Name == verbAttr.DefaultArgument select a).SingleOrDefault();
            //}
        }


        private void BuildArguments()
        {
            ParameterInfo[] parameters = Method.GetParameters();
            foreach (ParameterInfo parameter in parameters)
            {
                ArgumentModel argModel = null;
                ArgumentAttribute argAttribute = parameter.GetSingleAttribute<ArgumentAttribute>();

                if (argAttribute != null)
                {
                    argModel = new ArgumentModel(this.Program, this, parameter, argAttribute);
                }
                else
                {
                    argModel = new ArgumentModel(this.Program, this, parameter);
                }
                Arguments.Add(argModel);
            }
        }

        internal void Execute( object instance )
        {
            object[] argumentValues = new object[Arguments.Count];
            for (int i = 0; i < Arguments.Count; i++)
            {
                if ( Arguments[i].RuntimeValue == null )
                    argumentValues[i] = Arguments[i].Default;
                else
                    argumentValues[i] = Arguments[i].RuntimeValue;
            }

            Method.Invoke( instance, argumentValues);
        }

        //internal static VerbModel Create(MethodInfo method, Adlib.Console.Markup.ConsoleVerbAttribute verbAttribute)
        //{
        //    return new VerbModel(method, verbAttribute.VerbName) { Description = verbAttribute.Description };
        //}

        //internal static VerbModel Create(System.Reflection.MethodInfo method)
        //{
        //    return new VerbModel(method, "/" + method.Name);
        //}
    }
}
