using DragonSpark.Extensions;
using DragonSpark.Testing.Framework.Setup;
using DragonSpark.TypeSystem;
using Moq;
using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace DragonSpark.Testing.TypeSystem
{
	public class ApplicationAssemblyTransformerTests
	{
		[Theory, MoqAutoData]
		public void Basic( [Frozen]IAssemblyProvider provider, ApplicationAssemblyTransformer sut )
		{
			var mock = Mock.Get( provider );
			mock.Setup( p => p.Create() ).Returns( () => new[] { typeof(AutoDataAttribute), typeof(SetupAutoDataAttribute) }.Assemblies() );

			var assemblies = sut.Create( provider.Create() );
			
			mock.Verify( assemblyProvider => assemblyProvider.Create() );
			Assert.NotEqual( assemblies, provider.Create() );
		} 
	}
}