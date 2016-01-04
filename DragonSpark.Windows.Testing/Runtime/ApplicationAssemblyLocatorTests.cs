using DragonSpark.Testing.Framework.Setup;
using DragonSpark.Windows.Runtime;
using System;
using System.Reflection;
using Xunit;

namespace DragonSpark.Windows.Testing.Runtime
{
	public class ApplicationAssemblyLocatorTests
	{
		[Theory, ConfiguredMoqAutoData]
		public void Create( ApplicationAssemblyLocator sut )
		{
			var assembly = sut.Create();
			Assert.Equal( GetType().Assembly, assembly );
		}

		[Theory, ConfiguredMoqAutoData]
		public void Other( Assembly[] assemblies )
		{
			var sut = new ApplicationAssemblyLocator( assemblies, AppDomain.CreateDomain( "NotAnAssembly" ) );
			var assembly = sut.Create();
			Assert.Null( assembly );
		}
	}
}