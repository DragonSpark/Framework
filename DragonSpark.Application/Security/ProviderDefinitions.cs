using DragonSpark.Compose;
using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Application.Security
{
	public class ProviderDefinitions : Table<string, ProviderDefinition>, IProviderDefinitions
	{
		public ProviderDefinitions(params ProviderDefinition[] definitions)
			: base(definitions.ToOrderedDictionary(x => x.Name)) {}
	}
}
