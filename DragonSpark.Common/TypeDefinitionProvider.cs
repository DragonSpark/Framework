using DragonSpark.ComponentModel;

namespace DragonSpark.Common
{
	public class TypeDefinitionProvider : CompositeTypeDefinitionProvider
	{
		public TypeDefinitionProvider() : base( new ITypeDefinitionProvider[] { new ConventionBasedTypeDefinitionProvider(), new MetadataTypeDefinitionProvider() } )
		{}
	}
}