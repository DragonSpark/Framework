using DragonSpark.ComponentModel;
using DragonSpark.Testing.Server;

namespace DragonSpark.Testing.TestObjects
{
	public class TypeDefinitionProvider : CompositeTypeDefinitionProvider
	{
		public TypeDefinitionProvider() : base( new ITypeDefinitionProvider[] { new ConventionBasedTypeDefinitionProvider(), new MetadataTypeDefinitionProvider() } )
		{}
	}
}