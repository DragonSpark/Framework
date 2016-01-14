using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DragonSpark.Aspects;
using DragonSpark.Extensions;

namespace DragonSpark.ComponentModel
{
	public class CompositeTypeDefinitionProvider : ITypeDefinitionProvider
	{
		readonly IEnumerable<ITypeDefinitionProvider> providers;

		public CompositeTypeDefinitionProvider( IEnumerable<ITypeDefinitionProvider> providers )
		{
			this.providers = providers;
		}

		[Freeze]
		public TypeInfo GetDefinition( TypeInfo info )
		{
			var result = providers.FirstWhere( x => x.GetDefinition( info ) ) ?? info;
			return result;
		}
	}
}