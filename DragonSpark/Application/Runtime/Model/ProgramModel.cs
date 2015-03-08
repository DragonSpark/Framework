using DragonSpark.Application.Console.Markup;
using DragonSpark.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Application.Console.Model
{
    /// <summary>
    /// Represents a program at runtime.
    /// </summary>
    class ProgramModel
    {
        public Type ProgramType { get; private set; }

        public IList<VerbModel> Verbs { get; internal set; }

        public VerbModel DefaultVerb { get; private set; }

        public string ArgumentSwitch { get; private set; }

        public ProgramModel( Type type )
        {
            ProgramType = type;
            ArgumentSwitch = type.FromMetadata<ConsoleProgramAttribute, string>( x => x.ArgumentSwitch );
            Verbs = new List<VerbModel>();
            BuildVerbs();

            DefaultVerb = type.FromMetadata<ConsoleProgramAttribute, VerbModel>( x => Verbs.FirstOrDefault( y => y.Name == x.DefaultVerb ) );
        }

        void BuildVerbs()
        {
            var methods = ProgramType.GetMethods( DragonSparkBindingOptions.PublicProperties );
            foreach ( var method in methods )
            {
                var verbAttribute = method.GetSingleAttribute<VerbAttribute>();

                var verbModel = verbAttribute != null
                                        ? new VerbModel( this, method, verbAttribute )
                                        : new VerbModel( this, method );
                Verbs.Add( verbModel );
            }
        }
    }
}
