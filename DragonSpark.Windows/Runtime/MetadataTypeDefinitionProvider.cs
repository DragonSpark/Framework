using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace DragonSpark.Windows.Runtime
{
	public class MetadataTypeDefinitionProvider : AlterationBase<TypeInfo>, ITypeDefinitionProvider
	{
		public static MetadataTypeDefinitionProvider Default { get; } = new MetadataTypeDefinitionProvider();
		MetadataTypeDefinitionProvider() {}

		public override TypeInfo Get( TypeInfo parameter ) => parameter.GetCustomAttribute<MetadataTypeAttribute>().With( item => item.MetadataClassType.GetTypeInfo() );
	}
}
