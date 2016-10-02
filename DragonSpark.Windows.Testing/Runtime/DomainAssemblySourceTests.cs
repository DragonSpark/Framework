using DragonSpark.Windows.Runtime;
using System;
using System.Linq;
using Xunit;

namespace DragonSpark.Windows.Testing.Runtime
{
	public class DomainAssemblySourceTests
	{
		[Theory, Ploeh.AutoFixture.Xunit2.AutoData]
		public void GetAssemblies( DomainAssemblySource sut )
		{
			var assemblies = sut.Get( AppDomain.CurrentDomain );
			var loaded = AppDomain.CurrentDomain.GetAssemblies();
			Assert.NotEmpty( assemblies );
			Assert.True( assemblies.All( x => loaded.Contains( x ) ) );
		}

		[Fact]
		public void Instance()
		{
			Assert.NotNull( DomainAssemblySource.Default );
		}
	}
}