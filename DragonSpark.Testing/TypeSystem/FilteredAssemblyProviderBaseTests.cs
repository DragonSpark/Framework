using DragonSpark.Extensions;
using DragonSpark.Testing.Framework.Setup;
using DragonSpark.TypeSystem;
using Moq;
using Ploeh.AutoFixture.Xunit2;
using System.Linq;
using Xunit;

namespace DragonSpark.Testing.TypeSystem
{
	public class FilteredAssemblyProviderBaseTests
	{
		[Theory, MoqAutoData]
		public void Basic( [Frozen]IAssemblyProvider provider, FilteredAssemblyProviderBase sut )
		{
			var mock = Mock.Get( provider );
			mock.Setup( p => p.GetAssemblies() ).Returns( () => new[] { typeof(AutoDataAttribute), typeof(SetupAutoDataAttribute) }.Select( type => type.Assembly ).Fixed() );

			var assemblies = sut.GetAssemblies();
			
			mock.Verify( assemblyProvider => assemblyProvider.GetAssemblies() );
			Assert.NotEqual( assemblies, provider.GetAssemblies() );
		} 
	}
}