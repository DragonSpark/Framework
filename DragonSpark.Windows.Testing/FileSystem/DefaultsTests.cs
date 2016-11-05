using System;
using System.Collections.Immutable;
using System.Composition;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DragonSpark.Activation;
using DragonSpark.Activation.Location;
using DragonSpark.Application;
using DragonSpark.Application.Setup;
using DragonSpark.Composition;
using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Framework.Application;
using DragonSpark.Testing.Framework.Application.Setup;
using DragonSpark.Testing.Objects;
using DragonSpark.TypeSystem;
using DragonSpark.Windows.Runtime;
using DragonSpark.Windows.Testing.TestObjects;
using Moq;
using Ploeh.AutoFixture.Xunit2;
using Xunit;
using Xunit.Abstractions;
using Activator = DragonSpark.Activation.Activator;
using Attribute = DragonSpark.Testing.Objects.Attribute;

namespace DragonSpark.Windows.Testing.FileSystem
{
	/// <summary>
	/// This file can be seen as a bucket for all the testing done around setup.  It also can be seen as a huge learning bucket for xUnit and AutoFixture.  This does not contain best practices.  Always be learning. :)
	/// </summary>
	[Trait( Traits.Category, Traits.Categories.IoC ), ContainingTypeAndNested, FrameworkTypes]
	public class DefaultsTests : TestCollectionBase
	{
		public DefaultsTests( ITestOutputHelper output ) : base( output ) {}

		[Theory, DragonSpark.Testing.Framework.Application.AutoData]
		void RegisterInstanceGeneric( [Service]IServiceRepository registry, Class instance )
		{
			Assert.Null( GlobalServiceProvider.Default.Get<IInterface>() );

			registry.Add( instance );

			var located = GlobalServiceProvider.Default.Get<IInterface>();
			Assert.IsType<Class>( located );
			Assert.Same( instance, located );
		}

		[Theory, DragonSpark.Testing.Framework.Application.AutoData]
		void RelayedAttribute()
		{
			var attribute = typeof(Relayed).GetAttribute<Attribute>();
			Assert.Equal( "This is a relayed class attribute.", attribute.PropertyName );
		}

		[Theory, DragonSpark.Testing.Framework.Application.AutoData, AdditionalTypes( typeof(NormalPriority) )]
		public void GetAllTypesWith( [Service] ImmutableArray<Type> sut ) => Assert.True( sut.Decorated<PriorityAttribute>().Contains( typeof(NormalPriority) ) );

		[Theory, DragonSpark.Testing.Framework.Application.AutoData]
		public void Evaluate( ClassWithParameter sut ) => Assert.Equal( sut.Parameter, sut.Evaluate<object>( nameof(sut.Parameter) ) );

		[Theory, DragonSpark.Testing.Framework.Application.AutoData, AdditionalTypes( typeof(Class) )]
		public void RegisterGeneric()
		{
			var located = GlobalServiceProvider.Default.Get<IInterface>();
			Assert.IsType<Class>( located );
		}

		[Theory, DragonSpark.Testing.Framework.Application.AutoData]
		public void Mocked( [Frozen]Mock<IInterface> sut, IInterface item )
		{
			Assert.Equal( sut.Object, item );
		}

		[Theory, DragonSpark.Testing.Framework.Application.AutoData, IncludeParameterTypes( typeof(YetAnotherClass), typeof(Activator) )]
		public void Factory( [Service]AllTypesOfFactory sut )
		{
			var items = sut.Create<IInterface>();
			Assert.True( items.Any() );
			Assert.NotNull( items.FirstOrDefaultOfType<YetAnotherClass>() );
		}

		[Theory, DragonSpark.Testing.Framework.Application.AutoData, IncludeParameterTypes]
		public void Locate( [Service]DomainAssemblySpecification sut )
		{
			Assert.True( sut.IsSatisfiedBy( GetType().Assembly ) );
		}

		[Fact]
		public void ConventionLocator()
		{
			var type = ConventionImplementations.Default.Get( typeof(Assembly) );
			Assert.Null( type );

			Assert.True( new[] { typeof(Assembly), typeof(IActivator) }.All( Composition.Defaults.ConventionCandidate.IsSatisfiedBy ) );
		}

		[Theory, DragonSpark.Testing.Framework.Application.AutoData, IncludeParameterTypes( typeof(ApplicationAssembly) )]
		public void EnsureAssemblyResolvesAsExpected( [Service]Assembly assembly )
		{
			Assert.NotNull( assembly );
			Assert.Equal( GetType().Assembly, assembly );
		}

		[Theory, DragonSpark.Testing.Framework.Application.AutoData, IncludeParameterTypes( typeof(ApplicationAssembly), typeof(AssemblyInformationSource) )]
		public void CreateAssembly( [Service]IParameterizedSource<Assembly, AssemblyInformation> factory, [Service]IServiceProvider container, [Service]Assembly sut )
		{
			var fromFactory = ApplicationAssembly.Default.Get();
			var fromContainer = container.Get<Assembly>();
			Assert.Same( fromFactory, fromContainer );
			

			Assert.Same( fromContainer, sut );

			var fromProvider = factory.Get( fromFactory );
			Assert.NotNull( fromProvider );

			var assembly = GetType().Assembly;
			Assert.Equal( assembly.From<AssemblyTitleAttribute, string>( attribute => attribute.Title ), fromProvider.Title );
			Assert.Equal( assembly.From<AssemblyCompanyAttribute, string>( attribute => attribute.Company ), fromProvider.Company );
			Assert.Equal( assembly.From<AssemblyCopyrightAttribute, string>( attribute => attribute.Copyright ), fromProvider.Copyright );
			Assert.Equal( assembly.From<DebuggableAttribute, string>( attribute => "DEBUG" ), fromProvider.Configuration );
			Assert.Equal( assembly.From<AssemblyDescriptionAttribute, string>( attribute => attribute.Description ), fromProvider.Description );
			Assert.Equal( assembly.From<AssemblyProductAttribute, string>( attribute => attribute.Product ), fromProvider.Product );
			Assert.Equal( assembly.GetName().Version, fromProvider.Version );
		}

		[Theory, DragonSpark.Testing.Framework.Application.AutoData]
		void RegisterInterface( [Service]IAnotherInterface sut )
		{
			Assert.IsType<MultipleInterfaces>( sut );
		}

		[Export( typeof(IAnotherInterface) )]
		public class MultipleInterfaces : IInterface, IAnotherInterface, IItem {}
		public interface IItem {}

		interface IAnotherInterface {}
	}
}
