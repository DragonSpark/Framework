using DragonSpark.Activation.Location;
using DragonSpark.Composition;
using DragonSpark.Extensions;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Framework.Application;
using DragonSpark.Testing.Framework.Application.Setup;
using DragonSpark.TypeSystem;
using JetBrains.Annotations;
using System;
using System.Collections.Immutable;
using System.Composition;
using System.Composition.Convention;
using System.Composition.Hosting;
using System.Linq;
using System.Reflection;
using Xunit;
using Xunit.Abstractions;

namespace DragonSpark.Testing.Composition
{
	[Trait( Traits.Category, Traits.Categories.ServiceLocation )]
	public class ConventionBuilderTests : TestCollectionBase
	{
		public ConventionBuilderTests( ITestOutputHelper output ) : base( output ) {}

		[Theory, AutoData]
		public void BasicConvention( ContainerConfiguration configuration, ConventionBuilder sut )
		{
			sut.ForTypesMatching( DragonSpark.Specifications.Common.Always.IsSatisfiedBy ).Export();
			var types = this.Adapt().WithNested();
			var container = configuration.WithParts( types, sut ).CreateContainer();
			var export = container.GetExport<SomeExport>();
			Assert.NotNull( export );
			Assert.NotSame( export, container.GetExport<SomeExport>() );

			var invalid = container.TryGet<IItem>();
			Assert.Null( invalid );

			var shared = container.GetExport<SharedExport>();
			Assert.NotNull( shared );
			Assert.Same( shared, container.GetExport<SharedExport>() );
		}

		[Theory, AutoData, ContainingTypeAndNested]
		public void LocalData( ImmutableArray<Type> sut, ImmutableArray<Assembly> assemblies )
		{
			var items = sut.ToArray();

			var expected = GetType().Adapt().WithNested().Concat( FormatterTypesAttribute.Types ).Fixed();
			Assert.Equal( expected.Length, items.Length );
			Assert.Equal( expected.OrderBy( type => type.FullName ), items.OrderBy( type => type.FullName ) );

			var actual = assemblies.ToArray().ToImmutableHashSet();
			Assert.Equal( expected.Assemblies().ToImmutableHashSet(), actual );
			Assert.Contains( GetType().Assembly, actual );
		}

		[Theory, AutoData]
		public void LocalStrict( [Service]ISingletonLocator sut )
		{
			Assert.IsType<SingletonLocator>( sut );
		}

		[UsedImplicitly]
		interface IItem {}

		[UsedImplicitly]
		class SomeExport {}

		[UsedImplicitly, Shared]
		class SharedExport {}
	}
}
