using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Parameterized.Caching;
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
			var implementations = type.Adapt().GetImplementations( typeof(ISource<>) );
			if ( implementations.Any() )
			{
				yield return implementations.First().Adapt().GetInnerType();
			}
		}
	}

	public sealed class SourceAccountedValues : AlterationBase<object>
	{
		public static IParameterizedSource<Type, Func<object, object>> Defaults { get; } = new Cache<Type, Func<object, object>>( type => new SourceAccountedValues( type.Adapt().IsInstanceOfType ).ToCache().GetAssigned );

		readonly Func<object, bool> specification;

		[UsedImplicitly]
		public SourceAccountedValues( Func<object, bool> specification )
		{
			this.specification = specification;
		}

		public override object Get( object parameter ) => Candidates( parameter ).FirstOrDefault( specification );

		static IEnumerable<object> Candidates( object parameter )
		{
			yield return parameter;
			var source = parameter as ISource;
			var candidate = source?.Get();
			if ( candidate != null )
			{
				yield return candidate;
			}
		}
	}
}