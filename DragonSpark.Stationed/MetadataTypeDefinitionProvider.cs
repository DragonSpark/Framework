namespace DragonSpark.Stationed
{
	using System.ComponentModel.DataAnnotations;
	using System.Reflection;
	using ComponentModel;
	using Extensions;

	public class MetadataTypeDefinitionProvider : ITypeDefinitionProvider
	{
		public TypeInfo GetDefinition( TypeInfo info )
		{
			var result = info.FromMetadata<MetadataTypeAttribute, TypeInfo>( item => item.MetadataClassType.GetTypeInfo() );
			return result;
		}
	}
}
