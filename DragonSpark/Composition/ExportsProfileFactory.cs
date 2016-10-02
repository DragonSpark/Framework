using DragonSpark.Application;
using DragonSpark.Extensions;
using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace DragonSpark.Composition
{
	public sealed class ExportsProfileFactory : SourceBase<ExportsProfile>
	{
		readonly static Func<Type, ConventionMapping> Selector = ConventionMappingFactory.Default.Get;
		readonly static Func<Type, SingletonExport> SingletonExports = SingletonExportFactory.Default.Get;
		readonly static Func<Type, IEnumerable<Type>> Expander = ExportTypeExpander.Default.Get;

		public static ISource<ExportsProfile> Default { get; } = new Scope<ExportsProfile>( new ExportsProfileFactory().GlobalCache() );
		ExportsProfileFactory() : this( ApplicationTypes.Default.ToDelegate(), AppliedExportLocator.Default.ToSourceDelegate() ) {}

		readonly Func<ImmutableArray<Type>> typesSource;
		readonly Func<Type, AppliedExport> exportSource;

		ExportsProfileFactory( Func<ImmutableArray<Type>> typesSource, Func<Type, AppliedExport> exportSource )
		{
			this.typesSource = typesSource;
			this.exportSource = exportSource;
		}

		public override ExportsProfile Get()
		{
			var types = typesSource();
			
			var applied = new AppliedExports( types.SelectAssigned( exportSource ) );
			var appliedTypes = applied.All().ToArray();

			var mappings = new ConventionMappings( types.Except( appliedTypes ).SelectAssigned( Selector ).Distinct( mapping => mapping.InterfaceType ) );

			var all = appliedTypes.Concat( mappings.All() ).SelectMany( Expander ).Distinct().ToImmutableHashSet();

			var specification = new IsValidTypeSpecification( all );
			var selector = new ConstructorSelector( new IsValidConstructorSpecification( specification.IsSatisfiedBy ).IsSatisfiedBy );

			var implementations = mappings.Values.Fixed();
			var constructions = applied
				.GetClasses()
				.Union( implementations )
				.SelectAssigned( selector.Get )
				.ToImmutableDictionary( info => info.DeclaringType );

			var constructed = new ConstructedExports( constructions );

			var conventions = new ConventionExports( mappings.Keys, implementations.Intersect( constructions.Keys ) );

			var core = implementations.Except( constructions.Keys ).Union( applied.GetProperties() ).SelectAssigned( SingletonExports );
			
			var singletons = new SingletonExports( core.ToImmutableDictionary( export => export.Location.DeclaringType ) );

			var result = new ExportsProfile( constructed, conventions, singletons, specification );
			return result;
		}
	}
}