using DragonSpark.Testing.Framework.Setup;
using DragonSpark.Windows.Runtime;
using System;
using System.Linq;
using Xunit;

namespace DragonSpark.Windows.Testing.Runtime
{
	public class DomainAssemblySourceTests
	{
		[Theory, AutoData]
		public void GetAssemblies( DomainAssemblySource sut )
		{
			var assemblies = sut.Create();
			var loaded = AppDomain.CurrentDomain.GetAssemblies();
			Assert.NotEmpty( assemblies );
			Assert.True( assemblies.All( x => loaded.Contains( x ) ) );
		}

		[Fact]
		public void Instance()
		{
			Assert.NotNull( DomainAssemblySource.Instance );
		}
	}
}