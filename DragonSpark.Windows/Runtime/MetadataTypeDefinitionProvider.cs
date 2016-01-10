using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace DragonSpark.Windows.Runtime
{
	public class MetadataTypeDefinitionProvider : ITypeDefinitionProvider
	{
		public static MetadataTypeDefinitionProvider Instance { get; } = new MetadataTypeDefinitionProvider();

		MetadataTypeDefinitionProvider() {}

		public TypeInfo GetDefinition( TypeInfo info ) => info.From<MetadataTypeAttribute, TypeInfo>( item => item.MetadataClassType.GetTypeInfo() );
	}
}
