using DragonSpark.Testing.Framework.Setup;
using DragonSpark.Windows.Runtime;
using System;
using System.Reflection;
using Xunit;

namespace DragonSpark.Windows.Testing.Runtime
{
	public class ApplicationAssemblyLocatorTests
	{
		[Theory, AutoData]
		public void Create( ApplicationAssemblyLocator sut )
		{
			var assembly = sut.Create();
			Assert.Equal( GetType().Assembly, assembly );
		}

		[Theory, Ploeh.AutoFixture.Xunit2.AutoData]
		public void Other( Assembly[] assemblies )
		{
			var sut = new ApplicationAssemblyLocator( new DomainApplicationAssemblyLocator( AppDomain.CreateDomain( "NotAnAssembly" ) ), new TypeSystem.ApplicationAssemblyLocator( assemblies ) );
			var assembly = sut.Create();
			Assert.Null( assembly );
		}
	}
}