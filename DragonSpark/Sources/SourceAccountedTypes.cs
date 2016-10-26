using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Parameterized.Caching;
using DragonSpark.TypeSystem;
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
		public static IParameterizedSource<Type, IParameterizedSource<object>> Defaults { get; } = new Cache<Type, IParameterizedSource<object>>( type => new SourceAccountedValues( type.Adapt() ).ToCache() );

		readonly TypeAdapter requestedType;

		public SourceAccountedValues( TypeAdapter requestedType )
		{
			this.requestedType = requestedType;
		}

		public override object Get( object parameter )
		{
			foreach ( var candidate in Candidates( parameter ) )
			{
				if ( requestedType.IsInstanceOfType( candidate ) )
				{
					return candidate;
				}
			}
			return null;
		}

		static IEnumerable<object> Candidates( object parameter )
		{
			yield return parameter;
			var source = parameter as ISource;
			if ( source != null )
			{
				yield return source.Get();
			}
		}
	}
}