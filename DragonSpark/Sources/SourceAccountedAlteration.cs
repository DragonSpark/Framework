using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Scopes;
using DragonSpark.TypeSystem;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Sources
{
	public sealed class SourceAccountedAlteration : AlterationBase<object>
	{
		public static IParameterizedSource<Type, Func<object, object>> Defaults { get; } = 
			new ParameterizedSingletonScope<Type, Func<object, object>>( type => new SourceAccountedAlteration( type.Adapt() ).Get );

		readonly Func<Type, bool> assignable;
		readonly Func<object, bool> specification;

		public SourceAccountedAlteration( TypeAdapter adapter ) : this( adapter.IsAssignableFrom, adapter.IsInstanceOfType ) {}

		[UsedImplicitly]
		public SourceAccountedAlteration( Func<Type, bool> assignable, Func<object, bool> specification )
		{
			this.assignable = assignable;
			this.specification = specification;
		}

		public override object Get( object parameter ) => Candidates( parameter ).FirstOrDefault( specification );

		IEnumerable<object> Candidates( object parameter )
		{
			yield return parameter;
			var aware = parameter as ISourceAware;
			if ( aware != null && assignable( aware.SourceType ) )
			{
				var source = parameter as ISource;
				var candidate = source?.Get();
				if ( candidate != null )
				{
					yield return candidate;
				}
			}
		}
	}
}