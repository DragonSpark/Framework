using System.Reflection;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Framework.Parameters;
using DragonSpark.Testing.Framework.Setup;
using DragonSpark.TypeSystem;
using Microsoft.Practices.Unity;
using Xunit;

namespace DragonSpark.Testing.Runtime
{
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