using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Parameterized.Caching;
using DragonSpark.Specifications;
using DragonSpark.TypeSystem;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace DragonSpark.Sources
{
	public sealed class SourceAccountedTypes : FactoryCache<Type, ImmutableArray<TypeAdapter>>
	{
		public static IParameterizedSource<Type, Func<object, bool>> Specifications { get; } = 
			new ParameterizedScope<Type, Func<object, bool>>( Factory.GlobalCache<Type, Func<object, bool>>( type => new AdapterInstanceSpecification( Default.Get( type ).ToArray() ).ToCachedSpecification().IsSatisfiedBy ) );

		public static SourceAccountedTypes Default { get; } = new SourceAccountedTypes();
		SourceAccountedTypes() {}

		protected override ImmutableArray<TypeAdapter> Create( Type parameter ) => Candidates( parameter ).AsAdapters();

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
			new ParameterizedScope<Type, Func<object, object>>( Factory.GlobalCache<Type, Func<object, object>>( type => new SourceAccountedAlteration( type.Adapt() ).ToCache().GetAssigned ) );

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
			var source = parameter as ISource;
			if ( source != null && assignable( source.SourceType ) )
			{
				var candidate = source.Get();
				if ( candidate != null )
				{
					yield return candidate;
				}
			}
		}
	}
}