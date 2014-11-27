using DragonSpark.ComponentModel;

namespace DragonSpark.Stationed
{
	public class TypeDefinitionProvider : CompositeTypeDefinitionProvider
	{
		public TypeDefinitionProvider() : base( new ITypeDefinitionProvider[] { new ConventionBasedTypeDefinitionProvider(), new MetadataTypeDefinitionProvider() } )
		{}
	}
}