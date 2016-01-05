using DragonSpark.Testing.Framework.Setup;
using DragonSpark.Windows.Runtime;
using System;
using System.Linq;
using Xunit;

namespace DragonSpark.Windows.Testing.Runtime
{
	public class DomainAssemblyProviderTests
	{
		[Theory, AutoData]
		public void GetAssemblies( DomainAssemblyProvider sut )
		{
			var assemblies = sut.Create();
			var loaded = AppDomain.CurrentDomain.GetAssemblies();
			Assert.NotEmpty( assemblies );
			Assert.True( assemblies.All( x => loaded.Contains( x ) ) );
		}

		[Fact]
		public void Instance()
		{
			Assert.NotNull( DomainAssemblyProvider.Instance );
		}
	}
}