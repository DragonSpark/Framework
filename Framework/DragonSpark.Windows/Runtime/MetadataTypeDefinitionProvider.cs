using System.ComponentModel.DataAnnotations;
using System.Reflection;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;

namespace DragonSpark.Windows.Runtime
{
	public class MetadataTypeDefinitionProvider : ITypeDefinitionProvider
	{
		public TypeInfo GetDefinition( TypeInfo info )
		{
			var result = info.FromMetadata<MetadataTypeAttribute, TypeInfo>( item => item.MetadataClassType.GetTypeInfo() );
			return result;
		}
	}
}
