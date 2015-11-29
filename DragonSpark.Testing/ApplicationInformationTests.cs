using DragonSpark.Extensions;
using DragonSpark.Runtime;
using DragonSpark.Setup;
using DragonSpark.Testing.Framework;
using Microsoft.Practices.Unity;
using System;
using System.Diagnostics;
using System.Reflection;
using DragonSpark.Testing.TestObjects;
using Xunit;
using Xunit.Abstractions;

namespace DragonSpark.Testing
{
	public class ApplicationInformationTests
	{
		[Theory, Test, SetupAutoData]
		public void Create( [Located]ApplicationInformation sut )
		{
			Assert.NotNull( sut.AssemblyInformation );
			Assert.Equal( DateTimeOffset.Parse( "2/1/2016" ), sut.DeploymentDate.GetValueOrDefault() );
			Assert.Equal( "http://framework.dragonspark.us/testing", sut.CompanyUri.ToString() );
			var assembly = GetType().Assembly;
			Assert.Equal( assembly.FromMetadata<AssemblyTitleAttribute, string>( attribute => attribute.Title ), sut.AssemblyInformation.Title );
			Assert.Equal( assembly.FromMetadata<AssemblyCompanyAttribute, string>( attribute => attribute.Company ), sut.AssemblyInformation.Company );
			Assert.Equal( assembly.FromMetadata<AssemblyCopyrightAttribute, string>( attribute => attribute.Copyright ), sut.AssemblyInformation.Copyright );
			Assert.Equal( assembly.FromMetadata<DebuggableAttribute, string>( attribute => "DEBUG" ), sut.AssemblyInformation.Configuration );
			Assert.Equal( assembly.FromMetadata<AssemblyDescriptionAttribute, string>( attribute => attribute.Description ), sut.AssemblyInformation.Description );
			Assert.Equal( assembly.FromMetadata<AssemblyProductAttribute, string>( attribute => attribute.Product ), sut.AssemblyInformation.Product );
			Assert.Equal( assembly.GetName().Version, sut.AssemblyInformation.Version );
		}
	}

	public class RegistrationTests : Tests
	{
		public RegistrationTests( ITestOutputHelper output ) : base( output )
		{}

		[Theory, Test, SetupAutoData]
		public void Singleton( [Located] IUnityContainer sut )
		{
			var once = sut.Resolve<RegisterAsSingleton>();
			var twice = sut.Resolve<RegisterAsSingleton>();
			Assert.Same( once, twice );
		}

		[Theory, Test, SetupAutoData]
		public void Many( [Located] IUnityContainer sut )
		{
			var once = sut.Resolve<RegisterAsMany>();
			var twice = sut.Resolve<RegisterAsMany>();
			Assert.NotSame( once, twice );
		}
	}

	public class AssemblyInformationFactoryTests
	{
		[Theory, Test, SetupAutoData]
		public void Create( AssemblyInformationFactory factory, IUnityContainer container, IApplicationAssemblyLocator locator, [Located]Assembly sut )
		{
			var registered = container.IsRegistered<Assembly>();
			Assert.True( registered );
			
			var fromFactory = locator.Create();
			var fromContainer = container.Resolve<Assembly>();
			Assert.Same( fromFactory, fromContainer );

			Assert.Same( fromContainer, sut );

			var fromProvider = factory.Create( fromFactory );
			Assert.NotNull( fromProvider );

			var assembly = GetType().Assembly;
			Assert.Equal( assembly.FromMetadata<AssemblyTitleAttribute, string>( attribute => attribute.Title ), fromProvider.Title );
			Assert.Equal( assembly.FromMetadata<AssemblyCompanyAttribute, string>( attribute => attribute.Company ), fromProvider.Company );
			Assert.Equal( assembly.FromMetadata<AssemblyCopyrightAttribute, string>( attribute => attribute.Copyright ), fromProvider.Copyright );
			Assert.Equal( assembly.FromMetadata<DebuggableAttribute, string>( attribute => "DEBUG" ), fromProvider.Configuration );
			Assert.Equal( assembly.FromMetadata<AssemblyDescriptionAttribute, string>( attribute => attribute.Description ), fromProvider.Description );
			Assert.Equal( assembly.FromMetadata<AssemblyProductAttribute, string>( attribute => attribute.Product ), fromProvider.Product );
			Assert.Equal( assembly.GetName().Version, fromProvider.Version );
		}
	}

	public class AssembliesFactoryTests
	{
		[Theory, Test, SetupAutoData]
		public void Create( AssembliesFactory factory, IUnityContainer container, IAssemblyProvider provider, [Located]Assembly[] sut )
		{
			var registered = container.IsRegistered<Assembly[]>();
			Assert.True( registered );
			
			var fromFactory = factory.Create();
			var fromContainer = container.Resolve<Assembly[]>();
			Assert.Equal( fromFactory, fromContainer );

			var fromProvider = provider.GetAssemblies();
			Assert.Equal( fromContainer, fromProvider );

			Assert.Equal( fromContainer, sut );
		}
	}
}