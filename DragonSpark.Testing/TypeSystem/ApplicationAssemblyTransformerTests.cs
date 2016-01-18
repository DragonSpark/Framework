using DragonSpark.Extensions;
using DragonSpark.Testing.Objects;
using DragonSpark.Testing.Objects.Setup;
using DragonSpark.TypeSystem;
using Moq;
using Ploeh.AutoFixture.Xunit2;
using Xunit;
using AutoDataAttribute = Ploeh.AutoFixture.Xunit2.AutoDataAttribute;

namespace DragonSpark.Testing.TypeSystem
{
	public class ApplicationAssemblyTransformerTests
	{
		[Theory, DefaultSetup.AutoData, AssemblyProvider.Register]
		public void Basic( [Frozen]IAssemblyProvider provider, ApplicationAssemblyTransformer sut )
		{
			var mock = Mock.Get( provider );
			mock.Setup( p => p.Create() ).Returns( () => new[] { typeof(AutoDataAttribute), typeof(Framework.Setup.AutoDataAttribute) }.Assemblies() );

			var assemblies = sut.Create( provider.Create() );
			
			mock.Verify( assemblyProvider => assemblyProvider.Create() );
			Assert.NotEqual( assemblies, provider.Create() );
		} 
	}
}