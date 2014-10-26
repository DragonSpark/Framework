using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace DragonSpark.Testing.Server
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
