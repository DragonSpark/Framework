using DragonSpark.Application;
using DragonSpark.Extensions;
using System;
using System.Reflection;
using Xunit;
using ApplicationAssemblyLocator = DragonSpark.Windows.Runtime.ApplicationAssemblyLocator;

namespace DragonSpark.Windows.Testing.Runtime
{
	public class ApplicationAssemblyLocatorTests
	{
		[Theory, Ploeh.AutoFixture.Xunit2.AutoData]
		public void Create()
		{
			GetType().Yield().AsApplicationParts();
			Assert.Equal( GetType().Assembly, ApplicationAssembly.Default.Get() );
		}

		[Theory, Ploeh.AutoFixture.Xunit2.AutoData]
		public void Other( Assembly[] assemblies )
		{
			var sut = new ApplicationAssemblyLocator( AppDomain.CreateDomain( "NotAnAssembly" ) );
			var assembly = sut.Get( assemblies );
			Assert.Null( assembly );
		}
	}
}