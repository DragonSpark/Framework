using DragonSpark.Application;
using DragonSpark.Composition;
using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized.Caching;
using DragonSpark.Sources.Scopes;
using DragonSpark.Specifications;
using DragonSpark.TypeSystem;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace DragonSpark.Sources.Parameterized
{
	public sealed class SourceTypes : CacheWithImplementedFactoryBase<Type, Type>
	{
		readonly static string[] Suffixes = { "Source", "Factory" };
		readonly static Func<Type, KeyValuePair<Type, Type>?> Selector = Mappings.Implementation.Get;

		public static IParameterizedSource<Type, Type> Default { get; } = new ParameterizedSingletonScope<Type, Type>( x => new SourceTypes().ToDelegate() );
		SourceTypes() : this( ApplicationTypes.Default.Get().SelectAssigned( Selector ).ToImmutableDictionary() ) {}

		readonly IDictionary<Type, Type> mappings;

		SourceTypes( IDictionary<Type, Type> mappings )
		{
			this.mappings = mappings;
		}

		protected override Type Create( Type parameter )
		{
			var candidates = mappings.Introduce( parameter, tuple => tuple.Item2.Adapt().IsAssignableFrom( tuple.Item1.Value ) ).ToArray();
			var conventions = Suffixes.Introduce( parameter.Name, tuple => string.Concat( tuple.Item1, tuple.Item2 ) ).ToArray();
			var keyValuePairs = Get( parameter, candidates, conventions ).ToArray();
			var result = keyValuePairs.FirstAssigned()?.Key;
			return result;
		}

		static IEnumerable<KeyValuePair<Type, Type>?> Get( Type parameter, KeyValuePair<Type, Type>[] mappings, string[] conventions )
		{
			yield return mappings.Introduce( conventions, info => info.Item2.Contains( info.Item1.Key.Name ) ).Only().NullIfDefault();
			yield return mappings.Introduce( parameter, arg => arg.Item1.Value == arg.Item2 ).FirstOrDefault();
			yield return mappings.FirstOrDefault();
		}

		sealed class Mappings : ParameterizedSourceBase<Type, KeyValuePair<Type, Type>?>
		{
			readonly static Func<Type, Type> Results = ResultTypes.Default.ToDelegate();
			readonly static ISpecification<Type> Specification = Activation.Defaults.Instantiable.And( Defaults.KnownSourcesSpecification, ContainsExportSpecification.Default, new DelegatedSpecification<Type>( type => Results( type ) != typeof(object) ) );

			public static IParameterizedSource<Type, KeyValuePair<Type, Type>?> Implementation { get; } = new Mappings().Apply( Specification );
			Mappings() {}

			public override KeyValuePair<Type, Type>? Get( Type parameter ) =>
				new KeyValuePair<Type, Type>( parameter, Results( parameter ) );
		}
	}
}