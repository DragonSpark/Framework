using System.ComponentModel.DataAnnotations;
using System.Reflection;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.TypeSystem;

namespace DragonSpark.Windows.Runtime
{
	public class MetadataTypeDefinitionProvider : ITypeDefinitionProvider
	{
		readonly IAttributeProvider provider;

		public MetadataTypeDefinitionProvider( [Required]IAttributeProvider provider )
		{
			this.provider = provider;
		}

		public TypeInfo GetDefinition( TypeInfo info ) => provider.FromMetadata<MetadataTypeAttribute, TypeInfo>( info, item => item.MetadataClassType.GetTypeInfo() );
	}
}
