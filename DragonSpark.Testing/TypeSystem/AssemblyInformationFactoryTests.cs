using System.Diagnostics;
using System.Reflection;
using DragonSpark.Extensions;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Framework.Parameters;
using DragonSpark.Testing.Framework.Setup;
using DragonSpark.TypeSystem;
using Microsoft.Practices.Unity;
using Xunit;

namespace DragonSpark.Testing.TypeSystem
{
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
}