using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Framework.Parameters;
using DragonSpark.Testing.Framework.Setup;
using DragonSpark.TypeSystem;
using Xunit;

namespace DragonSpark.Testing.TypeSystem
{
	public class ApplicationAssemblyLocatorTests
	{
		[Theory, Test, SetupAutoData]
		public void Locate( [Located]ApplicationAssemblyLocator sut )
		{
			Assert.Same( GetType().Assembly, sut.Create() );
		} 
	}
}