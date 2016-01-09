using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.TypeSystem;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.Windows.Runtime
{
	public class TypeDefinitionProvider : CompositeTypeDefinitionProvider
	{
		public TypeDefinitionProvider( [Required]ConventionBasedTypeDefinitionProvider convention ) : base( new ITypeDefinitionProvider[] { convention, new MetadataTypeDefinitionProvider( AttributeProvider.Instance ) } )
		{}
	}
}