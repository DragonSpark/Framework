using DragonSpark.Application;
using DragonSpark.Extensions;
using DragonSpark.TypeSystem;
using DragonSpark.Windows.Runtime;
using System.Reflection;
using Xunit;

namespace DragonSpark.Windows.Testing.Runtime
{
	public class DomainAssemblySpecificationTests
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
			var sut = new DomainAssemblySpecification( typeof(object).Assembly );
			var locator = new ApplicationAssemblyLocator( sut.IsSatisfiedBy );
			var assembly = locator.Get( assemblies );
			Assert.Null( assembly );
		}
	}
}