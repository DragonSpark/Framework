using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DragonSpark.Extensions;

namespace DragonSpark.ComponentModel
{
	public class CompositeTypeDefinitionProvider : ITypeDefinitionProvider
	{
		readonly static Dictionary<TypeInfo, TypeInfo> MetadataCache = new Dictionary<TypeInfo, TypeInfo>();

		readonly IEnumerable<ITypeDefinitionProvider> providers;

		public CompositeTypeDefinitionProvider( IEnumerable<ITypeDefinitionProvider> providers )
		{
			this.providers = providers;
		}

		public TypeInfo GetDefinition( TypeInfo info )
		{
			var result = MetadataCache.Ensure( info, item => providers.Select( x => x.GetDefinition( info ) ).NotNull().FirstOrDefault() ?? info );
			return result;
		}
	}
}