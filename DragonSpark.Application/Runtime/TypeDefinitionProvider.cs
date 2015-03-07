using DragonSpark.ComponentModel;

namespace DragonSpark.Application.Runtime
{
	public class TypeDefinitionProvider : CompositeTypeDefinitionProvider
	{
		public TypeDefinitionProvider() : base( new ITypeDefinitionProvider[] { new ConventionBasedTypeDefinitionProvider(), new MetadataTypeDefinitionProvider() } )
		{}
	}
}