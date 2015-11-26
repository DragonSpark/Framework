using System.Diagnostics;
using DragonSpark.Runtime;
using DragonSpark.Testing.Framework;
using Microsoft.Practices.Unity;
using System.Reflection;
using DragonSpark.Extensions;
using Xunit;

namespace DragonSpark.Testing
{
	public class ApplicationInformationTests
	{
		[Theory, Test, SetupAutoData]
		public void Create( AssemblyInformation sut )
		{
			
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
			Assert.Same( fromFactory, fromContainer );

			var fromProvider = provider.GetAssemblies();
			Assert.Same( fromContainer, fromProvider );

			Assert.Same( fromContainer, sut );
		}
	}
}