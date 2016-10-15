using DragonSpark.ComponentModel;
using DragonSpark.TypeSystem;
using System.Reflection;
using Xunit;

namespace DragonSpark.Testing.ComponentModel
{
	public class TypeDefinitionsTests
	{
		[Fact]
		public void Coverage_EmptyConfiguration()
		{
			DragonSpark.TypeSystem.Configuration.TypeDefinitionProviders.Assign( () => Items<ITypeDefinitionProvider>.Immutable );
			Assert.Null( TypeDefinitions.Default.Get( GetType().GetTypeInfo() ) );
		}
	}
}