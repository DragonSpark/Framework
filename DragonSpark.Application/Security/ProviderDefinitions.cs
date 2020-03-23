using DragonSpark.Model.Selection.Stores;
using NetFabric.Hyperlinq;

namespace DragonSpark.Application.Security
{
	public class ProviderDefinitions : Table<string, ProviderDefinition>, IProviderDefinitions
	{
		public ProviderDefinitions(params ProviderDefinition[] definitions)
			: base(definitions.ToDictionary(x => x.Name)) {}
	}
}
