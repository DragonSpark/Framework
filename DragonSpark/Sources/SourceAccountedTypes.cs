using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Parameterized.Caching;
using DragonSpark.TypeSystem;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace DragonSpark.Sources
{
	public sealed class SourceAccountedTypes : FactoryCache<Type, ImmutableArray<Type>>
	{
		public static SourceAccountedTypes Default { get; } = new SourceAccountedTypes();
		SourceAccountedTypes() {}

		protected override ImmutableArray<Type> Create( Type parameter ) => Candidates( parameter ).ToImmutableArray();

		static IEnumerable<Type> Candidates( Type type )
		{
			yield return type;
			foreach ( var implementation in type.Adapt().GetImplementations( typeof(ISource<>) ) )
			{
				yield return implementation.Adapt().GetInnerType();
			}
		}
	}

	public sealed class SourceAccountedAlteration : AlterationBase<object>
	{
		public static IParameterizedSource<Type, Func<object, object>> Defaults { get; } = 
			new Curry<Type, object>( type => new SourceAccountedAlteration( type.Adapt() ).ToCache().GetAssigned ).Create();

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