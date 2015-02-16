using DragonSpark.ComponentModel;

namespace DragonSpark.Application
{
	public class TypeDefinitionProvider : CompositeTypeDefinitionProvider
	{
		public TypeDefinitionProvider() : base( new ITypeDefinitionProvider[] { new ConventionBasedTypeDefinitionProvider(), new MetadataTypeDefinitionProvider() } )
		{}
	}
}